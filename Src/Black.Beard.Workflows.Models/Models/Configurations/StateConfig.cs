using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{
    public class StateConfig
    {

        public string Name { get; set; }

        public string Label { get; set; }

        public Dictionary<string, IncomingEventConfig> Events { get; set; } = new Dictionary<string, IncomingEventConfig>();

        public List<ResultRuleConfig> IncomingRules { get; set; } = new List<ResultRuleConfig>();

        public List<ResultRuleConfig> OutcomingRules { get; set; } = new List<ResultRuleConfig>();

    }

}
