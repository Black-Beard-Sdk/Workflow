using Bb.Workflows;
using Bb.Workflows.Models;
using Bb.Workflows.Outputs;

namespace Bb.Workflows.Outputs.Mom
{



    public class PushBusActionOutputAction : OutputAction
    {

        public PushBusActionOutputAction(OutputAction child = null)
            : base(child)
        { }

        protected override void Prepare_Impl(RunContext ctx)
        {


        }

        protected override void Execute_Impl()
        {

        }

        protected override ITransaction BeginTransaction_Impl()
        {
            return new PushBusActionTransaction();
        }
    }

}
