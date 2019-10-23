using System;

namespace Bb.Workflows.Models.Configurations
{


    [System.Diagnostics.DebuggerDisplay("WHEN {RuleText} SWITCH {TargetStateName}")]
    public class InitializationConfig
    {

        public Func<RunContext, bool> Rule { get; set; }

        public string TargetStateName { get; set; }

        public StateConfig TargetState { get; internal set; }

        public string RuleText { get; internal set; }

    }

}
