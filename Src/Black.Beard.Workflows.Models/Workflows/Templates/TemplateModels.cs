using Bb.Workflows.Models;
using System;
using System.Linq;

namespace Bb.Workflows.Templates
{
    public class TemplateModels
    {

        public static Func<ResultAction, DynObject> Reminder = (r) =>
            DynObject
                .Create("Name", Constants.PushReminder)
                .Add("delay", WorkflowClock.Now().AddMinutes(r.Delay).ToString())
                .Add("canal", (ctx) => "IncomingEvent") // --read configuration where canal can be found
            ;


        public static Func<ResultAction, DynObject> __cancelReminderAction = (r) =>
            DynObject.Create("Uuid", (ctx) => Guid.NewGuid())
                .Add("CurrentState", (ctx) => ctx.Workflow.CurrentState)
                .Add("TaskUuid", (ctx) => $"{ctx.Workflow.ExternalId}_{ctx.Workflow.Events.Count() - 1}_{ctx.Workflow.LastEvent.FromState}_Expiration")
            ;


        public static Func<ResultAction, DynObject> __expired = (r) =>
            DynObject
                .Create("Uuid", (ctx) => $"{ctx.Workflow.ExternalId}_{ctx.Workflow.Events.Count()}_{ctx.Workflow.CurrentState}_Expiration")
                .Add("Name", Constants.Events.ExpiredEventName)
                .Add("WorkflowId", (ctx) => ctx.Workflow.Uuid)
                .Add("CurrentState", (ctx) => ctx.Workflow.CurrentState)
            ;


        public static Func<ResultAction, DynObject> DefaultAction = (r) =>
            DynObject
                .Create("Uuid", (ctx) => r.Uuid)
                .Add("Name", r.Name)
                .Add("WorkflowId", (ctx) => ctx.Workflow.Uuid)
                .Add("EventId", (ctx) => ctx.Event.Uuid)                 
                .Add("From_State", (ctx) => ctx.Event.FromState)
                .Add("To_State", (ctx) => ctx.Event.ToState)
                .Add("Arguments", r?.Arguments)
            ;

    }

}
