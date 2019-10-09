using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{

    public class TransitionConfig
    {


        public Func<RunContext, bool> WhenRule { get; set; }

        public string TargetStateName { get; set; }


        public List<ResultRuleConfig> RuleActions { get; set; } = new List<ResultRuleConfig>();
        public StateConfig TargetState { get; internal set; }
    }

}
