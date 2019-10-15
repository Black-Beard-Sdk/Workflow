﻿using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Configurations
{
    public class ResultRuleConfig
    {

        public Func<RunContext, bool> Rule { get; set; }

        public List<ResultActionConfig> Actions { get; set; } = new List<ResultActionConfig>();

    }

}
