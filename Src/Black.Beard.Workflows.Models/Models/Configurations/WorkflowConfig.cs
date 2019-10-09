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

            return _evaluateFilter(@event);

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
                    }

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

        private static DeclaredEventConfig GetEventDefinition( string key)
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

        private Func<IncomingEvent, bool> BuildIncomingAccessor()
        {

            var item = Expression.Variable(typeof(IncomingEvent), "item");
            var containsKey = typeof(DynamicObject).GetMethod("ContainsKey");
            var _this = typeof(DynamicObject).GetMethod("get_Item");
            var _value = typeof(DynamicObject).GetMethod("get_Value");
            var result = Expression.Variable(typeof(bool), "result");
            var property = typeof(IncomingEvent).GetProperty("ExtendedDatas");

            var _end = Expression.Label("end");

            List<Expression> blk = new List<Expression>();

            blk.Add(Expression.Assign(result, Expression.Constant(true)));

            foreach (var filter in this.Filters)
            {

                var p = Expression.Property(item, property);
                var a1 = Expression.Not( Expression.Call(p, containsKey, Expression.Constant(filter.Key)));

                var a2 = Expression.Call(p, _this, Expression.Constant(filter.Key));
                var a3 = Expression.Call(a2, _value);

                var a4 = Expression.NotEqual(a3, Expression.Constant(filter.Value));

                var i = Expression.IfThen
                (
                      Expression.ExclusiveOr(a1, a4)
                    , Expression.Block(Expression.Assign(result, Expression.Constant(false)), Expression.Goto(_end))

                );

                blk.Add(i);

            }

            blk.Add(Expression.Label(_end));
            blk.Add(result);

            var ldb = Expression.Lambda<Func<IncomingEvent, bool>>(Expression.Block(new ParameterExpression[] { result }, blk.ToArray()), item);

            return ldb.Compile();

        }

        private Func<IncomingEvent, bool> _evaluateFilter;
        private StateConfig _initState;
        private volatile object _lock = new object();

    }

}
