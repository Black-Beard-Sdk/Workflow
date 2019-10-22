using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Workflows.Models
{

    public class Event
    {

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public string ExternalId { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public DynObject ExtendedDatas { get; set; } = new DynObject();

        public string FromState { get; set; }

        public string ToState { get; set; }

        public IList<PushedAction> Actions { get; set; } = new List<PushedAction>();

        public bool GetAction(Guid uuid, out PushedAction a)
        {

            if (this._a == null)
                this._a = Actions.ToDictionary(c => c.Uuid);

            return this._a.TryGetValue(uuid, out a);

        }

        private Dictionary<Guid, PushedAction> _a;

    }

}
