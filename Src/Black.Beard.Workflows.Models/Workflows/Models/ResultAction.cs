using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{

    public class ResultAction
    {

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public int Delay { get; set; }


        public Event Event { get; set; }


        public DateTimeOffset EventDate { get; set; }

        public IDictionary<string, DynObject> Arguments { get; set; } = new Dictionary<string, DynObject>();

        public string Kind { get; internal set; }

    }



}
