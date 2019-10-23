using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{

    [System.Diagnostics.DebuggerDisplay("{RuleText}")]
    public class ResultRuleConfig
    {

        public Func<RunContext, bool> Rule { get; set; }

        public List<ResultActionConfig> Actions { get; set; } = new List<ResultActionConfig>();
    
        public string RuleText { get; set; }
    
    }

}
