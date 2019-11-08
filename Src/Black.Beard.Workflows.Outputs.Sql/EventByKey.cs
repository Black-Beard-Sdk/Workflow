using Bb.Workflows.Models;
using System;

namespace Bb.Workflows.Outputs.Sql
{
    public class EventByKey : Event
    {

        public Guid WorkflowUuid { get; set; }
        public string Tag { get; internal set; }
    }


}
