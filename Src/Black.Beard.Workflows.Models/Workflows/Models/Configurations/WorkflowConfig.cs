using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Bb.Workflows.Models.Configurations
{

    public class WorkflowConfig
    {

        public string Name { get; set; }

        public string Label { get; set; }

        public int Version { get; set; }

        public int Concurrency { get; set; } = 1;

        public Dictionary<string, string> Filters { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, InitializationOnEventConfig> Initializers { get; set; } = new Dictionary<string, InitializationOnEventConfig>();

        public Dictionary<string, DeclaredEventConfig> DeclaredEvents { get; set; } = new Dictionary<string, DeclaredEventConfig>();

        public Dictionary<string, StateConfig> States { get; set; } = new Dictionary<string, StateConfig>();

        public uint Crc { get; set; }

        internal bool EvaluateFilter(IncomingEvent @event)
        {

            if (_evaluateFilter == null)
                BuildWorkflow();

            RunContext ctx = new RunContext(null, @event);

            return _evaluateFilter(ctx);

        }

        public void BuildWorkflow()
        {

            if (_evaluateFilter == null)
                lock (_lock)
                    if (_evaluateFilter == null)
                    {
                        BuildAutorizedEvents();
                        BuildStates();
                        _evaluateFilter = BuildIncomingAccessor();
                        BuildTransitionLabels();
                    }

        }

        private void BuildTransitionLabels()
        {
            foreach (var s in this.States)
                foreach (var e in s.Value.Events)
                    foreach (var t in e.Value.Transitions)
                        t.Label = $"{s.Key} -> {t.TargetState}";
        }

        private void BuildAutorizedEvents()
        {

            foreach (var item in this.Initializers)
                if (!this.DeclaredEvents.ContainsKey(item.Key))
                    this.DeclaredEvents.Add(item.Key, GetEventDefinition(item.Key));

            foreach (var state in this.States)
                foreach (var e in state.Value.Events)
                    if (!this.DeclaredEvents.ContainsKey(e.Key))
                        this.DeclaredEvents.Add(e.Key, GetEventDefinition(e.Key));
        }

        private static DeclaredEventConfig GetEventDefinition(string key)
        {

            var label = string.Empty;
            switch (key)
            {

                case Constants.Events.ExpiredEventName:
                    label = "Expiration of the state";
                    break;

                default:
                    break;
            }

            var result = new DeclaredEventConfig()
            {
                Name = key,
                Label = label
            };

            return result;
        }

        private void BuildStates()
        {

            foreach (var item1 in this.Initializers)
                foreach (var item2 in item1.Value.Switchs)
                {
                    if (!this.States.TryGetValue(item2.TargetStateName, out StateConfig s2))
                        throw new Exceptions.InvalidArgumentNameMethodReferenceException($"Missing state name {item2.TargetStateName}");

                    item2.TargetState = s2;

                }

            foreach (var item1 in this.States)
                foreach (var item2 in item1.Value.Events)
                    foreach (var item3 in item2.Value.Transitions)
                    {
                        if (!this.States.TryGetValue(item3.TargetStateName, out StateConfig s2))
                            throw new Exceptions.InvalidArgumentNameMethodReferenceException($"Missing state name {item3.TargetStateName}");

                        item3.TargetState = s2;

                    }

        }

        private Func<RunContext, bool> BuildIncomingAccessor()
        {

            var item = Expression.Variable(typeof(RunContext), "item");
            var check = typeof(DynObject).GetMethod("Check");
            var result = Expression.Variable(typeof(bool), "result");
            var property1 = typeof(RunContext).GetProperty("IncomingEvent");
            var property2 = typeof(IncomingEvent).GetProperty("ExtendedDatas");

            var _end = Expression.Label("end");

            List<Expression> blk = new List<Expression>();

            blk.Add(Expression.Assign(result, Expression.Constant(true)));

            foreach (var filter in this.Filters)
            {
                var _property = Expression.Property( Expression.Property(item, property1), property2);
                var a1 = Expression.Call(_property, check, Expression.Constant(filter.Key), Expression.Constant(filter.Value), item);
                var i = Expression.IfThen(Expression.Not(a1), Expression.Block(Expression.Assign(result, Expression.Constant(false)), Expression.Goto(_end)));
                blk.Add(i);
            }

            blk.Add(Expression.Label(_end));
            blk.Add(result);

            var ldb = Expression.Lambda<Func<RunContext, bool>>(Expression.Block(new ParameterExpression[] { result }, blk.ToArray()), item);

            return ldb.Compile();

        }

        private Func<RunContext, bool> _evaluateFilter;
        private StateConfig _initState;
        private volatile object _lock = new object();

    }

}
