using System;
using System.Collections.Generic;
using System.Linq;

namespace Bb.Workflows.Models
{

    public class Event : IExtendedDatas
    {

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset EventDate { get; set; }

        public DynObject ExtendedDatas() { return _extendedDatas; }

        public string FromState { get; set; }

        public string ToState { get; set; }

        public ChangeEnum Change { get; set; }
        
        public IList<PushedAction> Actions { get; set; } = new List<PushedAction>();


        public bool GetAction(Guid uuid, out PushedAction a)
        {

            if (this._a == null)
                this._a = Actions.ToDictionary(c => c.Uuid);

            return this._a.TryGetValue(uuid, out a);

        }


        public Event AddActions(params PushedAction[] actions)
        {
            foreach (var action in actions)
                this.Actions.Add(action);
            return this;
        }

        private Dictionary<Guid, PushedAction> _a;
        private DynObject _extendedDatas = new DynObject();

    }

}
