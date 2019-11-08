using Bb.Dao;
using Bb.Workflows.Models;
using System;

namespace Bb.Workflows.Outputs.Sql
{
    public class EventObjectMapping : ObjectMapping
    {

        public EventObjectMapping(WorkflowFactory factory) : base(typeof(EventByKey))
        {

            this._factory = factory;

            Build("WorkflowEvents"
                , (nameof(Event.Uuid), "Uuid")
                , (nameof(Event.CreationDate), "CreationDate")
                , (nameof(Event.Name), "Name")
                , (nameof(Event.FromState), "FromState")
                , (nameof(Event.ToState), "ToState")
                , (nameof(Event.EventDate), "EventDate")
                , (nameof(EventByKey.WorkflowUuid), "WorkflowUuid")                
                , (nameof(EventByKey.Tag), "DatasWorkflow")
                );

        }

        internal override void Map<T>(DbDataReaderContext ctx, T result)
        {

            base.Map<T>(ctx, result);

            var w = (EventByKey)(object)result;
            var txt = ctx.GetString("Datas");

            if (!string.IsNullOrEmpty(txt))
                _factory.Serializer.Populate(w, txt);

        }

        private readonly WorkflowFactory _factory;

    }


}
