using System;

namespace Bb.Workflows
{
    public static class WorkflowClock
    {

        public static Func<DateTimeOffset> Now = () => DateTimeOffset.Now;

    }

}
