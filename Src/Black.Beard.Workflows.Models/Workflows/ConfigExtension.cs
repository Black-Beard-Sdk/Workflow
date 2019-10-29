using Bb.Workflows.Models;
using Bb.Workflows.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Workflows
{
    public static class ConfigExtension
    {

        public static WorkflowConfig AddFilter(this WorkflowConfig self, string eventName, string targetStateName, Func<RunContext, bool> rule)
        {

            if (!self.Initializers.TryGetValue(eventName, out InitializationOnEventConfig o))
                self.Initializers.Add(eventName, o = new InitializationOnEventConfig() { EventName = eventName });

            o.Switchs.Add(new InitializationConfig() { TargetStateName = targetStateName, WhenRule = rule });

            return self;
        }

        public static WorkflowConfig AddFilter(this WorkflowConfig self, string key, string value)
        {
            self.Filters.Add(key, value);
            return self;
        }

        public static InitializationOnEventConfig AddSwitch(this InitializationOnEventConfig self, string targetStateName, Func<RunContext, bool> rule = null, string ruleName = "", string code = "", RuleSpan whenRulePosition = null)
        {
            self.AddSwitch(new InitializationConfig() { TargetStateName = targetStateName, WhenRule = rule, WhenRuleText = ruleName, WhenRuleCode = code, WhenRulePosition = whenRulePosition });
            return self;
        }

        public static InitializationOnEventConfig AddSwitch(this InitializationOnEventConfig self, InitializationConfig @switch)
        {
            self.Switchs.Add(@switch);
            return self;
        }


        public static WorkflowConfig AddInitializer(this WorkflowConfig self, InitializationOnEventConfig initializer)
        {
            self.Initializers.Add(initializer.EventName, initializer);
            return self;
        }

        public static WorkflowConfig AddState(this WorkflowConfig self, StateConfig state)
        {
            self.States.Add(state.Name, state);
            return self;
        }

        public static IncomingEvent AddExtendedDatas(this IncomingEvent self, string key, string value)
        {
            self.ExtendedDatas.Items.Add(key, new DynObject().SetValue(value));
            return self;
        }

        public static StateConfig AddEvent(this StateConfig self, IncomingEventConfig e)
        {
            self.Events.Add(e.Name, e);
            return self;
        }

        public static IncomingEventConfig AddTransition(this IncomingEventConfig self, TransitionConfig t)
        {
            self.Transitions.Add(t);
            return self;
        }

        public static IncomingEventConfig AddAction(this IncomingEventConfig self, Func<RunContext, bool> func, ResultActionConfig act)
        {
            var r = self.Rules.FirstOrDefault(c => c.WhenRule == func);
            if (r == null)
            {
                r = new ResultRuleConfig()
                {
                    WhenRule = func,
                };
                self.Rules.Add(r);
            }
            r.Actions.Add(act);
            return self;
        }

        public static TransitionConfig AddAction(this TransitionConfig self, Func<RunContext, bool> func, params ResultActionConfig[] actions)
        {
            var r = self.RuleActions.FirstOrDefault(c => c.WhenRule == func);
            if (r == null)
            {
                r = new ResultRuleConfig()
                {
                    WhenRule = func,
                };
                self.RuleActions.Add(r);
            }
            r.Actions.AddRange(actions);
            return self;
        }

        public static StateConfig AddIncomingActions(this StateConfig self, Func<RunContext, bool> func, params ResultActionConfig[] actions)
        {
            var r = self.IncomingRules.FirstOrDefault(c => c.WhenRule == func);
            if (r == null)
            {
                r = new ResultRuleConfig()
                {
                    WhenRule = func,
                };
                self.IncomingRules.Add(r);
            }
            r.Actions.AddRange(actions);
            return self;
        }

        public static ResultActionConfig AddArgument(this ResultActionConfig self, string key, string value)
        {
            if (value is string s && s.StartsWith("@"))
                self.Arguments.Add(key, Expresssions.ExpressionDynobjectExtension.GetAccessors<RunContext>(s.Substring(1)));
            else
                self.Arguments.Add(key, (c) => value);
            return self;
        }

        public static ResultActionConfig AddArgument(this ResultActionConfig self, string key, Func<RunContext, object> func)
        {
            self.Arguments.Add(key, func);
            return self;
        }

        public static StateConfig AddOutcomingActions(this StateConfig self, Func<RunContext, bool> func, params ResultActionConfig[] actions)
        {
            var r = self.OutcomingRules.FirstOrDefault(c => c.WhenRule == func);
            if (r == null)
            {
                r = new ResultRuleConfig()
                {
                    WhenRule = func,
                };
                self.OutcomingRules.Add(r);
            }
            r.Actions.AddRange(actions);
            return self;
        }


    }

}
