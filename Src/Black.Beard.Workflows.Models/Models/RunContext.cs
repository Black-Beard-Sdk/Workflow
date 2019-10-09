using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{
    public class RunContext
    {

        public RunContext(Workflow workflow, IncomingEvent @event)
        {
            this.Workflow = workflow;
            this.IncomingEvent = @event;
            this.Event = @event.Map();

            this.PreviousEvent = this.Workflow.LastEvent;

            this.Workflow.Events.Add(this.Event);

        }

        public Workflow Workflow { get; }

        public IncomingEvent IncomingEvent { get; }

        public Dictionary<string, object> Arguments { get; set; }

        public Event Event { get; }

        public Event PreviousEvent { get; }

    }

}
