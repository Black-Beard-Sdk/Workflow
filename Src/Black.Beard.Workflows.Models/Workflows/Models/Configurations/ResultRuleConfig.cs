using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{

    [System.Diagnostics.DebuggerDisplay("{RuleText}")]
    public class ResultRuleConfig
    {

        public Func<RunContext, bool> WhenRule { get; set; }
        public string WhenRuleText { get; set; }

        public List<ResultActionConfig> Actions { get; set; } = new List<ResultActionConfig>();
        public string WhenRuleCode { get; set; }

        public RuleSpan WhenRulePosition { get; set; } = new RuleSpan();

    }

}
