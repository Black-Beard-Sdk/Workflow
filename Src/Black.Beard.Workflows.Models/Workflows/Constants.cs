using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.Workflows
{

    public static class Constants
    {

        public const string CurrentWorkflow = "_workflow";

        public const string SetValueActionName = "$_update";
        public const string PushActionName = "$_push";
        public const string PushReminder = "$_reminder";

        public const string EventId = "EventId";
        public const string TaskUuid = "TaskId";

        // 


        public static class Events
        {
            public const string ResultActionName = "$_result";
            public const string ExpiredEventName = "__expired";
            public const string CancelReminderAction = "__cancelReminderAction";
        }

        public static class Properties
        {
            public const string WorkflowId = "WorkflowId";
        }


    }


}
