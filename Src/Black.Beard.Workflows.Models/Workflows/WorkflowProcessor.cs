using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Configurations;
using Bb.Workflows.Models.Messages;
using Bb.Workflows.Outputs;
using Bb.Workflows.Templates;

namespace Bb.Workflows
{

    public abstract class WorkflowProcessor
    {

        public abstract void EvaluateEvent(IncomingEvent @event);

    }

    public class WorkflowProcessor<TContext> : WorkflowProcessor
    where TContext : RunContext, new()
    {

        public WorkflowProcessor(WorkflowsConfig config, Action<TContext> contextCreator = null)
        {
            this._config = config;
            this._contextCreator = contextCreator;

            // Build a responsability chain for evaluate where the event must be intégrated in the tree model
            this.AppendEvent = new ResponsabilityResultAction<TContext>(
                new ResponsabilityEventStandard<TContext>()
                );
        }

        public TemplateRepository Templates { get; set; } = new TemplateRepository();

        public MetadatRepository Metadatas { get; set; } = new MetadatRepository();

        public IWorkflowSerializer Serializer { get; set; }

        public Func<OutputAction> OutputActions { get; set; }

        public IServiceProvider Services { get; set; }

        public override void EvaluateEvent(IncomingEvent @event)
        {

            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            if (OutputActions == null)
                throw new ArgumentNullException(nameof(OutputActions));

            List<TContext> contexts = this.GetExistingContexts(@event).ToList();
            var items = contexts.ToLookup(c => c.Workflow.WorkflowName);
            contexts.AddRange(this.EvaluateNewWorkflow(@event, items));

            if (contexts.Any())
            {

                // Responsability chain for evaluate where in the tree, the event must to be added 
                foreach (var item in contexts)
                    this.AppendEvent.Eval(item);

                TranslateActions(contexts);

                var act = OutputActions();

                foreach (var item in contexts)
                    act.Prepare(item);

                using (var transaction = act.BeginTransaction())
                {
                    act.Execute();
                    transaction.Commit();
                }

            }

        }


        private void TranslateActions(List<TContext> contexts)
        {

            foreach (var context in contexts)
                foreach (ResultAction action in context.Actions)
                {

                    var body = Templates.Get(action.Name, action);
                    var header = Metadatas.Get(action.Name);

                    var raw = new MessageRaw()
                    {
                        Header = new MessageHeader(header) { },
                        Body = (MessageBlock)body.Resolve(context),
                    };

                    if (action.Delay > 0)
                    {

                        body = Templates.Get(Constants.PushReminder, new ResultAction()
                        {
                            Uuid = action.Uuid,
                            Delay = action.Delay,
                            Name = Constants.PushReminder,
                            Kind = Constants.PushActionName
                        })
                        .Add("Message", body);

                        header = Metadatas.Get(Constants.PushReminder);

                        raw = new MessageRaw()
                        {
                            Header = new MessageHeader(header) { },
                            Body = raw,
                        };
                    }

                    var t = new PushedAction()
                    {
                        Name = action.Name,
                        Uuid = action.Uuid,
                        Kind = action.Kind,
                        ExecuteMessage = new MessageRaw()
                        {
                            Header = new MessageHeader(header) { },
                            Body = (MessageBlock)body.Resolve(context),
                        },
                        Change = ChangeEnum.New,
                    };

                    action.Event.Actions.Add(t);

                }
        }

        public Func<string, List<Workflow>> LoadExistingWorkflowsByExternalId { get; set; }

        internal IEnumerable<TContext> GetExistingContexts(IncomingEvent @event)
        {

            if (LoadExistingWorkflowsByExternalId != null)
            {

                List<Workflow> workflows = LoadExistingWorkflowsByExternalId(@event.ExternalId);

                // if incoming event contains WorkflowId property it must be restricted on the specified workflowid
                if (@event.ExtendedDatas.Items.TryGetValue(Constants.Properties.WorkflowId, out DynObject d))
                {
                    Guid workflowId = Guid.Parse(d.GetValue(null)?.ToString());
                    workflows = workflows.Where(w => w.Uuid == workflowId).ToList();
                }

                foreach (Workflow item in workflows)
                    if (CheckAndResolveConfig(item, @event.Uuid, @event.Name, out WorkflowConfig config))
                    {

                        var ctx = CreateContext(item, @event);
                        ctx.Event.ToState = ctx.Event.FromState = ctx.Workflow.CurrentState;
                        EvaluateEventInCurrentWorkflow(config, ctx);

                        yield return ctx;

                    }

            }

        }

        private bool CheckAndResolveConfig(Workflow item, Guid uuid, string eventName, out WorkflowConfig config)
        {

            config = null;

            // an event can't be integrated twice 
            if (item.GetEvent(uuid, out _))
            {
                Trace.WriteLine($"Duplicated event {uuid} can't be integrated twice.", TraceLevel.Warning.ToString());
                return false;
            }

            // resolve config to use
            config = this._config.Get(item.WorkflowName, item.Version);
            if (config == null)
                throw new Exceptions.MissingConfigurationException($"{item.WorkflowName} V {item.Version}");

            // Check this event is managed by this configuration
            if (!config.DeclaredEvents.ContainsKey(eventName))
            {
                Trace.WriteLine($"{config.Name} don't accept event {eventName}");
                return false;
            }

            return true;

        }


        internal List<TContext> EvaluateNewWorkflow(IncomingEvent @event, ILookup<string, TContext> existingWorkflows)
        {

            var configs = this._config.Get(@event).ToList();
            if (configs.Count == 0)
                Trace.WriteLine(new { Message = $"no configuration state selected for event {@event.Uuid}", Event = @event });

            List<TContext> results = new List<TContext>();

            foreach (WorkflowConfig config in configs)
            {

                TContext ctx = EvaluateIfWorkflowMustBeCreated(@event, config);
                if (ctx == null)
                    continue;

                TContext last = null;
                if (existingWorkflows != null)
                    last = existingWorkflows[ctx.Workflow.WorkflowName].OrderBy(c => c.Workflow.Concurency).LastOrDefault();

                if (last == null)
                    results.Add(ExecuteTransitionAfterCreationOfNewWorkflow(config, ctx));

                else if (last.Workflow.Concurency <= config.Concurrency)
                {
                    ctx.Workflow.Concurency = last.Workflow.Concurency + 1;
                    results.Add(ExecuteTransitionAfterCreationOfNewWorkflow(config, ctx));
                }

            }

            return results;

        }

        private static TContext ExecuteTransitionAfterCreationOfNewWorkflow(WorkflowConfig config, TContext ctx)
        {

            var state = config.States[ctx.Workflow.CurrentState];
            ParseAndCollectRules(state.IncomingRules, ctx);

            if (ctx.Workflow.Recursive)
                EvaluateEventInCurrentWorkflow(config, ctx);

            return ctx;

        }

        private TContext EvaluateIfWorkflowMustBeCreated(IncomingEvent @event, WorkflowConfig config)
        {

            TContext ctx = null;
            string switchTo = string.Empty;

            if (config.Initializers.TryGetValue(@event.Name, out InitializationOnEventConfig initializationOnEventConfig))
            {

                Workflow wrk = new Workflow()
                {
                    Uuid = Guid.NewGuid(),
                    WorkflowName = config.Name,
                    Version = config.Version,
                    ExternalId = @event.ExternalId,
                    CreationDate = WorkflowClock.Now(),
                    LastUpdateDate = WorkflowClock.Now(),
                    ExtendedDatas = @event.ExtendedDatas.Clone(),
                    Concurency = 1,
                    Change = ChangeEnum.New,
                };

                ctx = CreateContext(wrk, @event);

                foreach (var sw in initializationOnEventConfig.Switchs)
                    if (Evaluate(sw, ctx))
                    {
                        ctx.Event.ToState = ctx.Event.FromState = sw.TargetStateName;
                        ctx.Workflow.Recursive = initializationOnEventConfig.Recursive;
                        break;
                    }

                if (string.IsNullOrEmpty(ctx.Event.FromState))
                    ctx = null;

            }

            return ctx;

        }

        /// <summary>
        /// This method evaluate event on the worflow and evaluate impact on it.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="ctx"></param>
        private static void EvaluateEventInCurrentWorkflow(WorkflowConfig config, TContext ctx)
        {

            StateConfig state = config.States[ctx.Event.FromState];

            if (state.Events.TryGetValue(ctx.IncomingEvent.Name, out IncomingEventConfig resultEvent))
            {

                // Collecte action on event append
                ParseAndCollectRules(resultEvent.Rules, ctx);

                foreach (TransitionConfig transition in resultEvent.Transitions)
                    if (EvaluateRule(transition, ctx))
                    {

                        ParseAndCollectRules(transition.RuleActions, ctx);    // Collecte action to execute on transition
                        ParseAndCollectRules(state.OutcomingRules, ctx);

                        state = transition.TargetState;
                        ctx.Event.ToState = state.Name;

                        ParseAndCollectRules(state.IncomingRules, ctx);

                        break;

                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ParseAndCollectRules(List<ResultRuleConfig> rules, TContext ctx)
        {

            foreach (var subRule in rules)
                if (Evaluate(subRule, ctx))
                    foreach (var action in subRule.Actions)
                    {
                        ResultAction m = action.Map(ctx);
                        ctx.Actions.Add(m);
                    }

        }

        #region RuleEvaluation

        private static bool EvaluateRule(TransitionConfig m, TContext ctx)
        {

            if (m.WhenRule == null)
                return true;

            ctx.CurrentEvaluation.WhenRuleText = m.WhenRuleText;
            ctx.CurrentEvaluation.WhenRuleCode = m.WhenRuleCode;
            ctx.CurrentEvaluation.WhenRulePosition = m.WhenRulePosition;
            bool result = false;
            try
            {
                result = m.WhenRule(ctx);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ctx.CurrentEvaluation.WhenRuleText = string.Empty;
                ctx.CurrentEvaluation.WhenRuleCode = string.Empty;
                ctx.CurrentEvaluation.WhenRulePosition = RuleSpan.None;
            }

            return result;
        }

        private bool Evaluate(InitializationConfig m, TContext ctx)
        {

            if (m.WhenRule == null)
                return true;

            ctx.CurrentEvaluation.WhenRuleText = m.WhenRuleText;
            ctx.CurrentEvaluation.WhenRuleCode = m.WhenRuleCode;
            ctx.CurrentEvaluation.WhenRulePosition = m.WhenRulePosition;
            bool result = false;
            try
            {
                result = m.WhenRule(ctx);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ctx.CurrentEvaluation.WhenRuleText = string.Empty;
                ctx.CurrentEvaluation.WhenRuleCode = string.Empty;
                ctx.CurrentEvaluation.WhenRulePosition = RuleSpan.None;
            }

            return result;
        }

        private static bool Evaluate(ResultRuleConfig m, TContext ctx)
        {

            if (m.WhenRule == null)
                return true;

            ctx.CurrentEvaluation.WhenRuleText = m.WhenRuleText;
            ctx.CurrentEvaluation.WhenRuleCode = m.WhenRuleCode;
            ctx.CurrentEvaluation.WhenRulePosition = m.WhenRulePosition;
            bool result = false;
            try
            {
                result = m.WhenRule(ctx);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                ctx.CurrentEvaluation.WhenRuleText = string.Empty;
                ctx.CurrentEvaluation.WhenRuleCode = string.Empty;
                ctx.CurrentEvaluation.WhenRulePosition = RuleSpan.None;
            }

            return result;

        }

        #endregion RuleEvaluation

        private TContext CreateContext(Workflow workflow, IncomingEvent @event)
        {

            TContext context = new TContext()
            {
                Serializer = this.Serializer,
            };

            context.Set(workflow, @event);

            this._contextCreator?.Invoke(context);

            return context;

        }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Diagnostics.DebuggerNonUserCode]
        private void Stop()
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }


        private readonly WorkflowsConfig _config;
        private readonly Action<TContext> _contextCreator;
        private readonly Responsability<TContext> AppendEvent;
    
    }

}
