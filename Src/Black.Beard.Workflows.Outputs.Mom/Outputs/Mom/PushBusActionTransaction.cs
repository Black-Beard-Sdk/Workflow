using Bb.Brokers;
using Bb.Workflows.Outputs;

namespace Bb.Workflows.Outputs.Mom
{


    public class PushBusActionTransaction : BaseTransaction
    {

        public PushBusActionTransaction(IBrokerPublisher publisher)
        {
            this._publisher = publisher;
            this._transaction = publisher.BeginTransaction();
        }

        protected override void Commit_Impl()
        {
            this._publisher.Commit();
        }

        protected override void Rollback_Impl()
        {
            this._publisher.Rollback();
        }

        private readonly IBrokerPublisher _publisher;
        private readonly Brokers.ITransaction _transaction;

    }

}
