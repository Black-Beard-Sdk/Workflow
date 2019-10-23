using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{

    [System.Diagnostics.DebuggerDisplay("ON EVENT {EventName}")]
    public class IncomingEventConfig
    {

        public string Name { get; set; }

        public List<TransitionConfig> Transitions { get; set; } = new List<TransitionConfig>();

        public List<ResultRuleConfig> Rules { get; set; } = new List<ResultRuleConfig>();

    }

}
