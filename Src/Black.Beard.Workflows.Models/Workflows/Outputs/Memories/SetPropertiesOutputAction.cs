using Bb.Workflows.Models;
using System.Linq;

namespace Bb.Workflows.Outputs
{
    public class SetPropertiesOutputAction : OutputAction
    {

        public SetPropertiesOutputAction(OutputAction child = null) : base(child)
        {

        }

        protected override ITransaction BeginTransaction_Impl()
        {
            return new DoNothingTransaction();
        }

        protected override void Prepare_Impl(RunContext ctx)
        {

            var act = ctx.Actions.Where(c => c.Kind == Constants.SetValueActionName).ToList();

            foreach (var item in act)
            {

                var key = item.Arguments["key"].GetValue(ctx).ToString();
                var value = item.Arguments["value"].GetValue(ctx).ToString();

                if (!ctx.Workflow.ExtendedDatas().ContainsKey(key))
                    ctx.Workflow.ExtendedDatas().Items.Add(key, new DynObject().SetValue(value));
                else
                    ctx.Workflow.ExtendedDatas().Items[key].SetValue(value);              

            }

        }

        protected override void Execute_Impl()
        {

        }


    }

}
