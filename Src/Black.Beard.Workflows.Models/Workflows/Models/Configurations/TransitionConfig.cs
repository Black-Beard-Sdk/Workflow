using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{

    [System.Diagnostics.DebuggerDisplay("WHEN {WhenRuleText} SWITCH {Label}")]
    public class TransitionConfig
    {

        public TransitionConfig()
        {

        }

        public Func<RunContext, bool> WhenRule { get; set; }

        public string TargetStateName { get; set; }


        public List<ResultRuleConfig> RuleActions { get; set; } = new List<ResultRuleConfig>();
        
        public StateConfig TargetState { get; internal set; }
        public string Label { get; internal set; }
        public string WhenRuleText { get; set; }
        public string WhenRuleCode { get; set; }
        public RuleSpan WhenRulePosition { get; set; }
    }

}
