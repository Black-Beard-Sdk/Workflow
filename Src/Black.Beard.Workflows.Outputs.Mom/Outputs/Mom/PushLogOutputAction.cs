using Bb.Workflows.Models;
using Bb.Workflows.Outputs;

namespace UnitTestWorkflow
{

    public class PushLogOutputAction : OutputAction
    {

        public PushLogOutputAction(OutputAction child = null)
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
            return new PushLogTransaction();
        }

    }

}
