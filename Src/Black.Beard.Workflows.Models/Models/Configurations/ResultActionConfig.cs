using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Workflows.Models.Configurations
{


    public class ResultRuleConfig
    {

        public Func<RunContext, bool> Rule { get; set; }

        public List<ResultActionConfig> Actions { get; set; } = new List<ResultActionConfig>();

    }


    public class ResultActionConfig
    {

        public string Name { get; set; }

        public string Label { get; set; }

        public int Delay { get; set; }

        public Dictionary<string, Func<object, object>> Arguments { get; set; } = new Dictionary<string, Func<object, object>>();

        public string Kind { get; set; }

        public ResultAction Map(RunContext ctx)
        {

            var result = new ResultAction()
            {
                Uuid = Guid.NewGuid(),
                Name = this.Name,
                Label = this.Name,
                Delay = this.Delay,
                Kind = Kind,
                EventDate = WorkflowClock.Now()
            };

            foreach (var item in this.Arguments)
                result.Arguments.Add(item.Key, Convert.ToString(item.Value(ctx)));

            return result;

        }

    }

}
