
using System.Transactions;

namespace Bb.Workflows.Outputs.Mom
{


    public class SqlServerActionTransaction : BaseTransaction
    {

        public SqlServerActionTransaction(Dao.Transaction transaction)
        {
            this._transaction = transaction;
        }

        protected override void Commit_Impl()
        {
            this._transaction.Commit();
        }

        protected override void Rollback_Impl()
        {
            this._transaction.Rollback();
        }

        private readonly Dao.Transaction _transaction;

    }

}
