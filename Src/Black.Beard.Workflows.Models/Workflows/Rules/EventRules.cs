using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bb.Workflows.Rules
{

    public static class EventRules
    {

        [System.ComponentModel.DisplayName("CountAndEvalEvent")]
        [System.ComponentModel.Description("count event type and check have at least specified count")]
        public static bool CountAndEvalEvent(RunContext ctx, string eventName, int count)
        {
            return ctx.Workflow.Events.Where(c => c.Name == eventName).Count() <= count;
        }

    }


}
