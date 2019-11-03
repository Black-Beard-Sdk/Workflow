using Bb.Workflows.Models;
using System;

namespace Black.Beard.Workflows.Outputs.Sql
{
    public class EventByKey : Event
    {

        public Guid WorkflowUuid { get; set; }

    }


}
