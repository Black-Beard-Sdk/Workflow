using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.Workflows
{

    public static class Constants
    {

        public const string SetValueActionName = "$_update";
        public const string PushActionName = "$_push";


        // 


        public static class Events
        {
            public const string ExpiredEventName = "__expired";
            public const string CancelReminderAction = "__canceReminderAction";
        }


    }


}
