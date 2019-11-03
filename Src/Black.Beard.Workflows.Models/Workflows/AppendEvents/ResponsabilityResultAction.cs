using System;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Messages;

namespace Bb.Workflows
{
    public class ResponsabilityResultAction<TContext> : Responsability<TContext>
        where TContext : RunContext, new()
    {

        public ResponsabilityResultAction(Responsability<TContext> child = null)
            : base(child)
        {

        }

        public override bool Check(TContext model)
        {
            return model.Event.Name == Constants.Events.ResultActionName;
        }

        protected override void Execute(TContext model)
        {

            var ev = model.Event;
            var eventId = ev.ExtendedDatas[Constants.EventId].GetValue(model).ToString();
            var taskId = ev.ExtendedDatas[Constants.TaskUuid].GetValue(model).ToString();

            if (model.Workflow.GetEvent(Guid.Parse(eventId), out Event e))
                if (e.GetAction(Guid.Parse(taskId), out PushedAction a))
                {
                    a.Change = ChangeEnum.New,
                    var i = model.IncomingEvent;
                    var m = new MessageRaw();
                    var body = new MessageBlock()
                        .Add(nameof(i.CreationDate), i.CreationDate.ToString())
                        .Add(nameof(i.EventDate), i.EventDate.ToString())
                        ;

                    foreach (var item in i.ExtendedDatas.Items)
                        body.Add(item.Key, item.Value.GetValue(model));

                    m.Body = body;
                    a.ResultMessage = m;

                }


        }

    }

}
