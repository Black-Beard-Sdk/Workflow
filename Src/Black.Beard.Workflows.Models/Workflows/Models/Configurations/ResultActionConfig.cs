using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Workflows.Models.Configurations
{

    public class ResultActionConfig
    {

        public string Name { get; set; }

        public string Label { get; set; }

        public int Delay { get; set; }

        public DynObject Arguments { get; set; } = new DynObject();

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
                EventDate = WorkflowClock.Now(),
                Event = ctx.Event,
            };

            foreach (var item in this.Arguments.Items)
                result.Arguments.Add(item.Key, item.Value);

            return result;

        }

    }

}
