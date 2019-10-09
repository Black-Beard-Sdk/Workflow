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

            var act = ctx.Event.Actions.Where(c => c.Kind == Constants.SetValueActionName).ToList();

            foreach (var item in act)
            {

                var key = item.Arguments["key"];
                var value = item.Arguments["value"];

                if (!ctx.Workflow.ExtendedDatas.Items.ContainsKey(key))
                    ctx.Workflow.ExtendedDatas.Items.Add(key, new DynamicObject() { Value = value });
                else
                    ctx.Workflow.ExtendedDatas.Items[key].Value = value;              

            }
        }

        protected override void Execute_Impl()
        {

        }


    }

}
