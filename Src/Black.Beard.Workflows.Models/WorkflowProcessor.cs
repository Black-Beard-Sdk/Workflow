using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Configurations;
using Bb.Workflows.Outputs;

namespace Bb.Workflows
{

    public class WorkflowProcessor
    {

        public WorkflowProcessor(WorkflowsConfig config)
        {
            this._config = config;
        }

        public Func<OutputAction> OutputActions { get; set; }

        public void EvaluateEvent(IncomingEvent @event)
        {

            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            if (OutputActions == null)
                throw new ArgumentNullException(nameof(OutputActions));

            List<RunContext> contexts = GetExistingContexts(@event).ToList();
            contexts.AddRange(EvaluateNewWorkflow(@event));


            if (contexts.Any())
            {
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

        public Func<string, List<Workflow>> LoadExistingWorkflows { get; set; }

        private IEnumerable<RunContext> GetExistingContexts(IncomingEvent @event)
        {

            if (LoadExistingWorkflows != null)
            {

                var workflows = LoadExistingWorkflows(@event.ExternalId);

                foreach (Workflow item in workflows)
                {

                    var config = this._config.Get(item.WorkflowName, item.Version);

                    if (!config.DeclaredEvents.ContainsKey(@event.Name))
                    {
                        Trace.WriteLine($"{config.Name} don't accept event {@event.Name}");
                        continue;
                    }

                    var ctx = new RunContext(item, @event);
                    ctx.Event.ToState = ctx.Event.FromState = ctx.PreviousEvent.ToState;

                    EvaluateEventInCurrentWorkflow(config, ctx);

                    yield return ctx;

                }


            }

        }

        private List<RunContext> EvaluateNewWorkflow(IncomingEvent @event)
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

                EvaluateEventInCurrentWorkflow(config, ctx);

                results.Add(ctx);

            }

            return results;

        }

        private static RunContext EvaluateInitialization(IncomingEvent @event, WorkflowConfig config)
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
                };
                ctx = new RunContext(wrk, @event);

                foreach (var sw in initializationOnEventConfig.Switchs)
                    if (sw.Rule == null || sw.Rule(ctx))
                    {
                        ctx.Event.ToState = ctx.Event.FromState = sw.TargetStateName;
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
                        ctx.Event.Actions.Add(action.Map(ctx));

        }

        private readonly WorkflowsConfig _config;

    }

}
