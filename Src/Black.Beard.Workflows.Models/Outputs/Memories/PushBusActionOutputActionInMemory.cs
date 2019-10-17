using Bb.Workflows.Models;
using System.Linq;

namespace Bb.Workflows.Outputs
{

    public class PushBusActionOutputActionInMemory : OutputAction
    {

        public PushBusActionOutputActionInMemory(MemoryStorage storage, OutputAction child = null) 
            : base(child)
        {
            this._storage = storage;
        }

        protected override ITransaction BeginTransaction_Impl()
        {
            return new DoNothingTransaction();
        }

        protected override void Execute_Impl()
        {

            foreach (PushedAction item in this.Items)
                this._storage.Save<PushedAction>(item.Uuid, item);

        }

        protected override void Prepare_Impl(RunContext ctx)
        {
            var actions = ctx.Event.Actions.Where(c => c.Kind == Constants.PushActionName).ToList();
            this.Items.AddRange(actions);
        }

        private readonly MemoryStorage _storage;

    }

}
