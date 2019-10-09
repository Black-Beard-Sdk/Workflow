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

        public DateTimeOffset EventDate { get; set; }

        public IDictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
        public string Kind { get; internal set; }

        public PushedAction Map(Event e, Workflow w)
        {

            var push = new PushedAction()
            {
                Name = Name,
                WorkflowId = w.Uuid,
                EventId = e.Uuid,
                Uuid = Uuid,
            };

            foreach (var item in this.Arguments)
                push.Arguments.Add((item.Key, item.Value));

            push.Arguments.Add(("From_State", e.FromState));
            push.Arguments.Add(("To_State", e.ToState));

            return push;

        }

    }



}
