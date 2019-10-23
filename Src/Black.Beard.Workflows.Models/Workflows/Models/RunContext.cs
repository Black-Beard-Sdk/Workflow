using System;
using System.Collections.Generic;
using Bb.Workflows.Converters;
using Bb.Workflows.Models.Configurations;
using Bb.Workflows.Models.Logs;
using Bb.Workflows.Templates;

namespace Bb.Workflows.Models
{

    public class RunContext
    {

        public RunContext()
        {

        }

        public RunContext Set(Workflow workflow, IncomingEvent @event)
        {

            this.IncomingEvent = @event;
            this.Event = @event.Map();

            if (workflow != null)
            {
                this.Workflow = workflow;
                this.PreviousEvent = this.Workflow.LastEvent;
                this.Workflow.Events.Add(this.Event);
            }

            return this;

        }

        public Workflow Workflow { get; private set; }

        public IncomingEvent IncomingEvent { get; private set; }

        public Event Event { get; private set; }

        public Event PreviousEvent { get; private set; }

        public DynObject ExtendedDatas { get; set; } = new DynObject();
        
        public IWorkflowSerializer Serializer { get; internal set; }

        public IList<ResultAction> Actions { get; } = new List<ResultAction>();
        
        public List<MessageText> FunctionalLog { get; } = new List<MessageText>();

        public CurrentEvaluationDatas CurrentEvaluation { get; internal set; } = new CurrentEvaluationDatas();


    }

    public class CurrentEvaluationDatas
    {
        
        public string WhenRuleText { get; internal set; }
        
        public string WhenRuleCode { get; internal set; }
        
        public RuleSpan WhenRulePosition { get; internal set; } = RuleSpan.None;

    }

}
