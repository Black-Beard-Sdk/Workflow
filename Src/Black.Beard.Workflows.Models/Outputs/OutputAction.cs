using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;

namespace Bb.Workflows.Outputs
{


    public abstract class OutputAction
    {

        public OutputAction(OutputAction child = null)
        {
            this.child = child;
            this.Items = new List<dynamic>();
        }


        public void Prepare(RunContext ctx)
        {
            Trace.WriteLine($"Preparing {this.GetType().Name} on {ctx.Workflow.Uuid} impacted by {ctx.IncomingEvent.Uuid} ({ctx.IncomingEvent.Name})");
            Prepare_Impl(ctx);
            if (child != null)
                child.Prepare(ctx);
        }


        public void Execute()
        {
            Trace.WriteLine($"executing {this.GetType().Name}");
            Execute_Impl();
            child?.Execute();
        }


        public ITransaction BeginTransaction()
        {
            var trans = BeginTransaction_Impl();
            if (child != null)
                trans.Child = child.BeginTransaction();
            return trans;
        }



        protected abstract void Prepare_Impl(RunContext ctx);

        protected abstract ITransaction BeginTransaction_Impl();

        protected abstract void Execute_Impl();



        protected List<object> Items { get; }


        private OutputAction child;

    }

}
