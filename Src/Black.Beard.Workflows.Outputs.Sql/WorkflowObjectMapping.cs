using Bb.Dao;
using Bb.Workflows.Models;

namespace Black.Beard.Workflows.Outputs.Sql
{
    public class WorkflowObjectMapping : ObjectMapping
    {

        public WorkflowObjectMapping() : base(typeof(Workflow))
        {

            Build("Workflows"
                , (nameof(Workflow.Uuid), "Uuid")
                , (nameof(Workflow.Concurency), "Concurency")
                , (nameof(Workflow.CreationDate), "CreationDate")
                , (nameof(Workflow.LastUpdateDate), "LastUpdateDate")
                , (nameof(Workflow.ExternalId), "ExternalId")
                , (nameof(Workflow.Version), "WorkflowVersion")
                , (nameof(Workflow.WorkflowName), "WorkflowName")
                );

        }

        internal override void Map<T>(DbDataReaderContext ctx, T result)
        {

            base.Map<T>(ctx, result);

            var w = (Workflow)(object)result;



        }

    }


}
