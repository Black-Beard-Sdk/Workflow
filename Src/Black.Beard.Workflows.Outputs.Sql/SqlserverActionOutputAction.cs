using System.Linq;
using Bb.Workflows.Models;
using Black.Beard.Workflows.Outputs.Sql;

namespace Bb.Workflows.Outputs.Mom
{

    public class SqlserverActionOutputAction : OutputAction
    {

        public SqlserverActionOutputAction(WorkflowStoreSql store, OutputAction child = null)
            : base(child)
        {
            this._store = store;
        }

        protected override void Prepare_Impl(RunContext ctx)
        {
            this._workflow = ctx.Workflow;
        }

        protected override void Execute_Impl()
        {
            this._store.Save(_workflow);
        }

        public string PublisherName { get; set; }

        protected override ITransaction BeginTransaction_Impl()
        {

            return new SqlServerActionTransaction( _store.GetTransaction());

        }

        private readonly WorkflowStoreSql _store;
        private Workflow _workflow;
    
    }

}
