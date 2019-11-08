using Bb.Dao;
using Bb.Workflows.Models;

namespace Bb.Workflows.Outputs.Sql
{

    public class WorkflowObjectMapping : ObjectMapping
    {

        public WorkflowObjectMapping(WorkflowFactory factory) : base(typeof(Workflow))
        {

            this._factory = factory;

            Build("Workflows"
                , (nameof(Workflow.Uuid), "Uuid")
                , (nameof(Workflow.Version), "WorkflowVersion")
                , (nameof(Workflow.Concurency), "Concurency")
                , (nameof(Workflow.ExternalId), "ExternalId")
                , (nameof(Workflow.CreationDate), "CreationDate")
                , (nameof(Workflow.WorkflowName), "WorkflowName")
                , (nameof(Workflow.LastUpdateDate), "LastUpdateDate")
                );

        }


        internal override void Map<T>(DbDataReaderContext ctx, T result)
        {
            base.Map<T>(ctx, result);
            var w = (Workflow)(object)result;
        }


        public override T Create<T>(DbDataReaderContext ctx)
        {

            var workflowName = ctx.GetString("WorkflowName");
            var worflowVersion = ctx.GetInt32("WorkflowVersion");

            var result = _factory.CreateNewWorkflow(workflowName, worflowVersion.Value, string.Empty, null);

            return (T)(object)result;

        }

        private readonly WorkflowFactory _factory;

    }


}
