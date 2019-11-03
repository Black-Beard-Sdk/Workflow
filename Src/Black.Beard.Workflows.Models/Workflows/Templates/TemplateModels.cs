using Bb.Workflows.Models;
using System;
using System.Linq;

namespace Bb.Workflows.Templates
{
    
    public static class TemplateModels
    {

        public static Func<ResultAction, DynObject> DefaultAction = (r) =>
            DynObject
                .Create("Uuid", (ctx) => r.Uuid)
                .Add("Name", r.Name)
                .Add("PushedAt", (ctx) => WorkflowClock.Now)
                .AddSub("Arguments", r?.Arguments)
                    .Add("WorkflowId", (ctx) => ctx.Workflow.Uuid)
                    .Add("EventId", (ctx) => ctx.Event.Uuid)
                    .Add("From_State", (ctx) => ctx.Event.FromState)
                    .Add("To_State", (ctx) => ctx.Event.ToState)
                
            ;

    }

}
