using System;
using System.Linq;
using Bb.Brokers;
using Bb.Workflows;
using Bb.Workflows.Models;
using Bb.Workflows.Outputs;

namespace Bb.Workflows.Outputs.Mom
{



    public class PushBusActionOutputAction : OutputAction
    {

        public PushBusActionOutputAction(OutputAction child = null)
            : base(child)
        { }

        protected override void Prepare_Impl(RunContext ctx)
        {
            var actions = ctx.Event.Actions.Where(c => c.Kind == Constants.PushActionName).ToList();
            this.Items.AddRange(actions);
        }

        protected override void Execute_Impl()
        {
            var publisher = GetPublisher();
            foreach (PushedAction item in this.Items)
                publisher.Publish(item);
        }

        private IBrokerPublisher GetPublisher()
        {
            if (this._publisher == null)
                this._publisher = this.Brokers.CreatePublisher(this.PublisherName);
            return this._publisher;
        }

        public IFactoryBroker Brokers { get; set; }

        public string PublisherName { get; set; }

        protected override ITransaction BeginTransaction_Impl()
        {
            return new PushBusActionTransaction(GetPublisher());
        }

        private IBrokerPublisher _publisher;

    }

}
