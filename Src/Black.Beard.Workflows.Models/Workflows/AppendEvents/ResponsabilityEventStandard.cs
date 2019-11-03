using Bb.Workflows.Models;

namespace Bb.Workflows
{

    public class ResponsabilityEventStandard<TContext> : Responsability<TContext>
        where TContext : RunContext, new()

    {

        public ResponsabilityEventStandard(Responsability<TContext> child = null) 
            : base(child)
        {

        }
        public override bool Check(TContext model)
        {
            return true;
        }

        protected override void Execute(TContext model)
        {
            model.Workflow.Events.Add(model.Event);
        }

    }

}
