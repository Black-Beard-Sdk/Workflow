using System;

namespace Bb.Workflows
{
    public static class WorkflowClock
    {

        static WorkflowClock()
        {
            WorkflowClock._span = TimeSpan.Zero;
        }

        public static Func<DateTimeOffset> Now = () => DateTimeOffset.Now.Add(_span);
        private static TimeSpan _span;

        public static void Add(TimeSpan span)
        {
            _span = _span.Add(span);
        }

        public static void Reset()
        {
            WorkflowClock._span = TimeSpan.Zero;
        }

    }

}
