using System;

namespace Bb.Workflows.Models.Configurations
{
    public class InitializationConfig
    {

        public Func<RunContext, bool> Rule { get; set; }

        public string TargetStateName { get; set; }
        public StateConfig TargetState { get; internal set; }
    }

}
