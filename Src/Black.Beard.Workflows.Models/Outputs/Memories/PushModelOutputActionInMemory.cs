using Bb.Workflows.Models;

namespace Bb.Workflows.Outputs
{
    public class PushModelOutputActionInMemory : OutputAction
    {

        public PushModelOutputActionInMemory(MemoryStorage storage, OutputAction child = null) : base(child)
        {
            this._storage = storage;
        }

        protected override ITransaction BeginTransaction_Impl()
        {
            return new DoNothingTransaction();
        }

        protected override void Execute_Impl()
        {
            foreach (Workflow item in this.Items)
                this._storage.Save<Workflow>(item.Uuid, item);
        }

        protected override void Prepare_Impl(RunContext ctx)
        {
            this.Items.Add(ctx.Workflow);
        }

        private readonly MemoryStorage _storage;

    }

}
