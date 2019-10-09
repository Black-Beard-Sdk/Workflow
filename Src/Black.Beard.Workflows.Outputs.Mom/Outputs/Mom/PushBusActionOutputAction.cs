using Bb.Workflows;
using Bb.Workflows.Models;
using Bb.Workflows.Outputs;

namespace UnitTestWorkflow
{



    public class PushBusActionOutputAction : OutputAction
    {

        public PushBusActionOutputAction(OutputAction child = null)
            : base(child)
        { }

        protected override void Prepare_Impl(RunContext ctx)
        {
            foreach (var action in ctx.Event.Actions)
            {
                var e = action.Map(ctx.Event, ctx.Workflow);
                e.PushedAt = Bb.Workflows.WorkflowClock.Now();

                var txt = this.Serializer.Serialize(e);

                if (action.Delay > 0)
                {

                    var startingAfter = WorkflowClock.Now()
                        .AddMinutes(action.Delay)
                        ;

#warning convertir en message delai 

                }
                else
                    this.Items.Add(txt);
            }
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
