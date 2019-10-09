using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{
    public class IncomingEventConfig
    {

        public string Name { get; set; }

        public List<TransitionConfig> Transitions { get; set; } = new List<TransitionConfig>();

        public List<ResultRuleConfig> Rules { get; set; } = new List<ResultRuleConfig>();

    }

}
