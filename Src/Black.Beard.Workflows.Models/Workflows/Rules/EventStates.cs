using Bb.Workflows.Models;
using System.Linq;

namespace Bb.Workflows.Rules
{
    public static class EventStates
    {

        [System.ComponentModel.DisplayName("CountAndEvalState")]
        [System.ComponentModel.Description("count state type and check have at least specified count")]
        public static bool CountAndEvalState(RunContext ctx, string stateName, int count)
        {
            return ctx.Workflow.Events.Where(c => c.FromState == stateName).Count() <= count;
        }

    }


}
