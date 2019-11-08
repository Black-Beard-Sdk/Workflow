using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Workflows.Models
{

    public class Workflow : IExtendedDatas
    {

        public Guid Uuid { get; set; }

        public string ExternalId { get; set; }

        public int Concurency { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset LastUpdateDate { get; set; }

        public string WorkflowName { get; set; }

        public int Version { get; set; }

        public Event LastEvent { get => Events.Count > 0 ? Events[Events.Count - 1] : null; }

        public string CurrentState { get => Events.Count > 0 ? Events[Events.Count - 1].ToState : string.Empty; }

        public DynObject ExtendedDatas() { return _extendedDatas; }

        public IList<Event> Events { get; set; } = new List<Event>();

        public bool Recursive { get; internal set; }

        public ChangeEnum Change { get; set; }

        public bool GetEvent(Guid uuid, out Event e)
        {

            if (this._e == null)
                this._e = Events.ToDictionary(c => c.Uuid);

            return this._e.TryGetValue(uuid, out e);

        }

        
        public Workflow AddEvents(params Event[] events)
        {
            foreach (var @event in events)
                this.Events.Add(@event);
            return this;
        }

        private Dictionary<Guid, Event> _e;
        private DynObject _extendedDatas = new DynObject();

    }

    public enum ChangeEnum
    {

        None,
        New,
        Updated,

    }

}
