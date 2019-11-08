using System;

namespace Bb.Workflows.Models.Configurations
{


    [System.Diagnostics.DebuggerDisplay("WHEN {WhenRuleText} SWITCH {TargetStateName}")]
    public class InitializationConfig
    {

        public Func<RunContext, bool> WhenRule { get; set; }

        public string TargetStateName { get; set; }

        public StateConfig TargetState { get; internal set; }

        public string WhenRuleText { get; internal set; }
        public string WhenRuleCode { get; internal set; }
        public RuleSpan WhenRulePosition { get; internal set; }
    }

}
