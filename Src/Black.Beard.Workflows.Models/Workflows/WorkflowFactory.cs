using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bb.Workflows
{

    public abstract class WorkflowFactory
    {

        public Workflow CreateNewWorkflow(string workflowName, int workflowVersion, string externalkey, DynObject extendedDatas)
        {

            Workflow wrk = GetWorkflow(workflowName, workflowVersion);

            wrk.Uuid = Guid.NewGuid();
            wrk.WorkflowName = workflowName;
            wrk.Version = workflowVersion;
            wrk.ExternalId = externalkey;
            wrk.CreationDate = WorkflowClock.Now();
            wrk.LastUpdateDate = WorkflowClock.Now();
            wrk.Concurency = 1;
            wrk.Change = ChangeEnum.New;
            
            if (extendedDatas != null)
                wrk.ExtendedDatas().Merge(extendedDatas);

            return wrk;

        }

        protected abstract Workflow GetWorkflow(string workflowName, int version);

        public abstract RunContext CreateBaseContext(Workflow workflow, IncomingEvent incomingEvent);

        public abstract Event CreateBaseEvent(IncomingEvent incomingEvent);

        public IWorkflowSerializer Serializer { get; set; }

        public abstract IncomingEvent CreateBaseIncomingEvent(string payload, string type);

    }

    public class WorkflowFactory<TContext> : WorkflowFactory
        where TContext : RunContext, new()
    {


        public WorkflowFactory(Func<IncomingEvent> newIncomingEvent, Func<Workflow> newWorkflow)
        {

            this._workflows = new List<BoxWorflow>();
            this._Dictionaryworkflows = new Dictionary<string, BoxWorflow>();
            this._DictionaryIncomingEvent = new Dictionary<string, Func<IncomingEvent>>();

            if (newIncomingEvent == null)
                newIncomingEvent = () => new IncomingEvent();

            if (newWorkflow == null)
                newWorkflow = () => new Workflow();

            this._defaultWorkflow = newWorkflow;
            this._defaultIncomingEvent = newIncomingEvent;

        }


        public WorkflowFactory<TContext> Add(string key, Func<IncomingEvent> incomingEventCreator)
        {
            this._DictionaryIncomingEvent.Add(key, incomingEventCreator);
            return this;
        }


        public WorkflowFactory<TContext> Add(string workflowName, int version, Func<Workflow> workflowCreator)
        {

            var box = new BoxWorflow(workflowName, version, workflowCreator);
            this._Dictionaryworkflows.Add(box.Key, box);

            var i = _workflows.FirstOrDefault(c => c.WorkflowName == workflowName);
            if (i != null)
                if (i.Creator().GetType() != workflowCreator().GetType())
                    this._workflows.Add(box);

            return this;
        
        }


        public override IncomingEvent CreateBaseIncomingEvent(string payload, string incominType)
        {

            IncomingEvent incomingEvent = null;

            if (string.IsNullOrEmpty(incominType))
            {

                if (!this._DictionaryIncomingEvent.TryGetValue(incominType, out Func<IncomingEvent> i))
                    incomingEvent = i();

                else
                    throw new NotImplementedException($"no incoming event registered for {incominType}");

            }
            else
                incomingEvent = this._defaultIncomingEvent();

            Serializer.Populate(incomingEvent, payload);

            return incomingEvent;

        }


        public override RunContext CreateBaseContext(Workflow workflow, IncomingEvent incomingEvent)
        {
            return CreateContext(workflow, incomingEvent);
        }


        public override Event CreateBaseEvent(IncomingEvent incomingEvent)
        {

            var result = new Event()
            {
                Uuid = Guid.NewGuid(),
                EventDate = incomingEvent.EventDate,
                Name = incomingEvent.Name,
                CreationDate = WorkflowClock.Now(),
                Change = ChangeEnum.New,
            };
            result.ExtendedDatas().Merge(incomingEvent.ExtendedDatas());

            return result;

        }


        public TContext CreateContext(Workflow workflow, IncomingEvent incomingEvent)
        {

            TContext context = new TContext()
            {
                Serializer = this.Serializer,
            };

            context.Set(workflow, incomingEvent, this);

            if (ContextCreator != null)
                ContextCreator(context);

            return context;

        }


        public Action<TContext> ContextCreator { get; set; }


        protected override Workflow GetWorkflow(string workflowName, int version)
        {

            Func<Workflow> func = this._defaultWorkflow;

            string key = $"{workflowName}-{version}";

            if (this._Dictionaryworkflows.TryGetValue(key, out BoxWorflow box))
                func = box.Creator;

            else
            {

                if (this._workflowLookup == null)
                    this._workflowLookup = _workflows.ToLookup(c => c.WorkflowName);

                var w = this._workflowLookup[workflowName];

                if (w.Count() > 1)
                    func = w.First().Creator;

            }

            return func();

        }


        private readonly Dictionary<string, Func<IncomingEvent>> _DictionaryIncomingEvent;
        private readonly Dictionary<string, BoxWorflow> _Dictionaryworkflows;
        private readonly List<BoxWorflow> _workflows;
        private readonly Func<Workflow> _defaultWorkflow;
        private Func<IncomingEvent> _defaultIncomingEvent;
        private ILookup<string, BoxWorflow> _workflowLookup;

        private class BoxWorflow
        {

            public BoxWorflow(string workflowName, int Version, Func<Workflow> creator)
            {

                this.WorkflowName = workflowName;
                this.Creator = creator;

                this.Key = $"{WorkflowName}-{Version}";

            }

            public string Key { get; }

            public string WorkflowName { get; }

            public int Version { get; }

            public Func<Workflow> Creator { get; }

        }

    }


}

