using Bb.Dao;
using Bb.Workflows.Models;

namespace Black.Beard.Workflows.Outputs.Sql
{
    public class EventObjectMapping : ObjectMapping
    {

        public EventObjectMapping() : base(typeof(EventByKey))
        {

            Build("WorkflowEvents"
                , (nameof(Event.Uuid), "Uuid")
                , (nameof(Event.CreationDate), "CreationDate")
                , (nameof(Event.Name), "Name")
                , (nameof(Event.FromState), "FromState")
                , (nameof(Event.ToState), "ToState")
                , (nameof(Event.EventDate), "EventDate")
                , (nameof(EventByKey.WorkflowUuid), "WorkflowUuid")
                );

        }

        internal override void Map<T>(DbDataReaderContext ctx, T result)
        {

            base.Map<T>(ctx, result);

            var w = (Event)(object)result;

        }

    }


}
