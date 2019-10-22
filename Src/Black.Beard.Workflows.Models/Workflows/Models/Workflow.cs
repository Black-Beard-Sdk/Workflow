using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Workflows.Models
{

    public class Workflow
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

        public DynObject ExtendedDatas { get; set; } = new DynObject();

        public IList<Event> Events { get; set; } = new List<Event>();

        public bool Recursive { get; internal set; }

        public bool GetEvent(Guid uuid, out Event e)
        {

            if (this._e == null)
                this._e = Events.ToDictionary(c => c.Uuid);

            return this._e.TryGetValue(uuid, out e);

        }

        private Dictionary<Guid, Event> _e;

    }

}
