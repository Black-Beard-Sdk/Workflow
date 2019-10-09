using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{
    public class PushedAction
    {


        public PushedAction()
        {

        }

        public Guid WorkflowId { get; set; }

        public Guid EventId { get; set; }

        public Guid Uuid { get; set; }

        public DateTimeOffset PushedAt { get; set; }

        public string Name { get; set; }

        public List<(string, string)> Arguments { get; set; } = new List<(string, string)>();

    }



}
