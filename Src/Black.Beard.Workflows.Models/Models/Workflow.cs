using System;
using System.Collections.Generic;

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

        public DynamicObject ExtendedDatas { get; set; } = new DynamicObject();

        public IList<Event> Events { get; set; } = new List<Event>();


    }

}
