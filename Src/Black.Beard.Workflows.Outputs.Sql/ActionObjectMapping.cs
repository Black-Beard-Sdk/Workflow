using Bb.Dao;
using Bb.Workflows.Models;

namespace Black.Beard.Workflows.Outputs.Sql
{
    public class ActionObjectMapping : ObjectMapping
    {

        public ActionObjectMapping() : base(typeof(ActionByKey))
        {

            Build("WorkflowEvents"
                , (nameof(PushedAction.Name), "Name")
                , (nameof(PushedAction.Uuid), "Uuid")
                , (nameof(PushedAction.CancelMessage), "CancelMessage")
                , (nameof(PushedAction.ExecuteMessage), "ExecuteMessage")
                , (nameof(PushedAction.ResultCancelMessage), "ResultCancelMessage")
                , (nameof(PushedAction.ResultMessage), "ResultMessage")
                , (nameof(ActionByKey.EventUuid), "EventUuid")
                );

        }

        internal override void Map<T>(DbDataReaderContext ctx, T result)
        {

            base.Map<T>(ctx, result);

            var w = (Event)(object)result;

        }

    }


}
