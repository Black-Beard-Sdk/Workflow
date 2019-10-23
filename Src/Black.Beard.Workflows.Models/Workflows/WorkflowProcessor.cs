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

    public class WorkflowProcessor
    {

        public WorkflowProcessor(WorkflowsConfig config)
        {
            this._config = config;
        }

        public TemplateRepository Templates { get; set; } = new TemplateRepository();

        public MetadatRepository Metadatas { get; set; } = new MetadatRepository();

        public IWorkflowSerializer Serializer { get; set; }

        public Func<OutputAction> OutputActions { get; set; }

        public void EvaluateEvent(IncomingEvent @event)
        {

            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            if (OutputActions == null)
                throw new ArgumentNullException(nameof(OutputActions));

            List<RunContext> contexts = GetExistingContexts(@event).ToList();
            var items = contexts.ToLookup(c => c.Workflow.WorkflowName);
            contexts.AddRange(EvaluateNewWorkflow(@event, items));

            if (contexts.Any())
            {
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

        private void TranslateActions(List<RunContext> contexts)
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
                    };

                    action.Event.Actions.Add(t);

                }
        }

        public Func<string, List<Workflow>> LoadExistingWorkflowsByExternalId { get; set; }

        private IEnumerable<RunContext> GetExistingContexts(IncomingEvent @event)
        {

            if (LoadExistingWorkflowsByExternalId != null)
            {



                List<Workflow> workflows = LoadExistingWorkflowsByExternalId(@event.ExternalId);

                // if incoming event contains WorkflowId it restrict on the specific workflow
                if (@event.ExtendedDatas.Items.TryGetValue(Constants.Properties.WorkflowId, out DynObject d))
                {
                    Guid workflowId = Guid.Parse(d.GetValue(null)?.ToString());
                    workflows = workflows.Where(w => w.Uuid == workflowId).ToList();
                }

                foreach (Workflow item in workflows)
                {

                    var config = this._config.Get(item.WorkflowName, item.Version);

                    if (!config.DeclaredEvents.ContainsKey(@event.Name))
                    {
                        Trace.WriteLine($"{config.Name} don't accept event {@event.Name}");
                        continue;
                    }

                    var ctx = new RunContext(item, @event)
                    {
                        Serializer = this.Serializer,
                    };

                    ctx.Event.ToState = ctx.Event.FromState = ctx.PreviousEvent.ToState;

                    EvaluateEventInCurrentWorkflow(config, ctx);

                    yield return ctx;

                }


            }

        }

        private List<RunContext> EvaluateNewWorkflow(IncomingEvent @event, ILookup<string, RunContext> existingWorkflows)
        {

            var configs = this._config.Get(@event).ToList();
            if (configs.Count == 0)
                Trace.WriteLine(new { Message = $"no configuration state selected for event {@event.Uuid}", Event = @event });

            List<RunContext> results = new List<RunContext>();

            foreach (WorkflowConfig config in configs)
            {

                RunContext ctx = EvaluateInitialization(@event, config);
                if (ctx == null)
                    continue;

                RunContext last = null;
                if (existingWorkflows != null)
                    last = existingWorkflows[ctx.Workflow.WorkflowName].OrderBy(c => c.Workflow.Concurency).LastOrDefault();

                if (last == null)
                    results.Add(ExecuteTransitionOnNewWorkflow(config, ctx));

                else if (last.Workflow.Concurency <= config.Concurrency)
                {
                    ctx.Workflow.Concurency = last.Workflow.Concurency + 1;
                    results.Add(ExecuteTransitionOnNewWorkflow(config, ctx));
                }

            }

            return results;

        }

        private static RunContext ExecuteTransitionOnNewWorkflow(WorkflowConfig config, RunContext ctx)
        {
            var state = config.States[ctx.Workflow.CurrentState];
            ParseAndCollectRules(state.IncomingRules, ctx);

            if (ctx.Workflow.Recursive)
                EvaluateEventInCurrentWorkflow(config, ctx);

            return ctx;
        }

        private RunContext EvaluateInitialization(IncomingEvent @event, WorkflowConfig config)
        {

            RunContext ctx = null;
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
                };

                ctx = new RunContext(wrk, @event)
                {
                    Serializer = this.Serializer,
                };


                foreach (var sw in initializationOnEventConfig.Switchs)
                    if (sw.Rule == null || sw.Rule(ctx))
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

        private static void EvaluateEventInCurrentWorkflow(WorkflowConfig config, RunContext ctx)
        {

            StateConfig state = config.States[ctx.Event.FromState];

            if (state.Events.TryGetValue(ctx.IncomingEvent.Name, out IncomingEventConfig resultEvent))
            {

                // Collecte action on event append
                ParseAndCollectRules(resultEvent.Rules, ctx);

                foreach (TransitionConfig transition in resultEvent.Transitions)
                    if (transition.WhenRule == null || transition.WhenRule(ctx))
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
        private static void ParseAndCollectRules(List<ResultRuleConfig> rules, RunContext ctx)
        {

            foreach (var subRule in rules)
                if (subRule.Rule == null || subRule.Rule(ctx))
                    foreach (var action in subRule.Actions)
                    {
                        ResultAction m = action.Map(ctx);
                        ctx.Actions.Add(m);
                    }

        }


        [System.Diagnostics.DebuggerStepThrough]
        [System.Diagnostics.DebuggerNonUserCode]
        private void Stop()
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }


        private readonly WorkflowsConfig _config;

    }

}
