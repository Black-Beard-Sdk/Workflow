using Bb.Workflows.Models;
using Bb.Workflows.Outputs;

namespace UnitTestWorkflow
{

    public class PushModelOutputAction : OutputAction
    {

        public PushModelOutputAction(OutputAction child = null)
            : base(child)
        { }

        protected override void Prepare_Impl(RunContext ctx)
        {
            var model = ctx.Serializer.Serialize(ctx.Workflow);
            base.Items.Add(model);
        }

        protected override void Execute_Impl()
        {

        }

        protected override ITransaction BeginTransaction_Impl()
        {
            return new PushModelTransaction();
        }
    }


}
