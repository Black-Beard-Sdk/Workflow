using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{

    public class Event
    {

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public string ExternalId { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public DynamicObject ExtendedDatas { get; set; } = new DynamicObject();

        public string FromState { get; set; }

        public string ToState { get; set; }

        public IList<ResultAction> Actions { get; set; } = new List<ResultAction>();

    }

}
