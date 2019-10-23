﻿using System;
using System.Collections.Generic;
using Bb.Workflows.Converters;
using Bb.Workflows.Templates;

namespace Bb.Workflows.Models
{
    public class RunContext
    {

        public RunContext(Workflow workflow, IncomingEvent @event)
        {

            this.IncomingEvent = @event;
            this.Event = @event.Map();

            if (workflow != null)
            {
                this.Workflow = workflow;
                this.PreviousEvent = this.Workflow.LastEvent;
                this.Workflow.Events.Add(this.Event);
            }

        }

        public Workflow Workflow { get; }

        public IncomingEvent IncomingEvent { get; }

        public Event Event { get; }

        public Event PreviousEvent { get; }

        public DynObject ExtendedDatas { get; set; } = new DynObject();
        
        public IWorkflowSerializer Serializer { get; internal set; }

        public IList<ResultAction> Actions { get; } = new List<ResultAction>();
    }

}