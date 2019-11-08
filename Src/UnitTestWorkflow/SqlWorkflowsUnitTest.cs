using Bb.Dao;
using Bb.Workflows;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Messages;
using Bb.Workflows.Outputs.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace UnitTestWorkflow
{

    [TestClass]
    public partial class SqlWorkflowsUnitTest
    {

        public SqlWorkflowsUnitTest()
        {


        }

        [TestMethod]
        public void InitializeWorkflow()
        {

            var config = new SqlManagerConfiguration()
            {
                ConnectionString = "Data Source=L00280\\SQLEXPRESS;Initial Catalog=Workflow;Integrated Security=True",
                ProviderInvariantName = "SqlClient"
            };

            DbProviderFactories.RegisterFactory(config.ProviderInvariantName, SqlClientFactory.Instance);

            var serializer = new PartialJsonWorkflowSerializer();

            WorkflowFactory factory = new WorkflowFactory<RunContext>(null, null)
            {
                Serializer = serializer,
            };


            WorkflowStoreSql store = new WorkflowStoreSql(new Bb.Dao.SqlManager(config), factory);



            var w = new Workflow()
            {
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                Change = ChangeEnum.New,
                Concurency = 1,
                CreationDate = DateTimeOffset.Now,
                Version = 1,
                WorkflowName = "work1",
            }.AddEvents
            (
                new Event()
                {
                    Uuid = Guid.NewGuid(),
                    Name = "Event1",
                    Change = ChangeEnum.New,
                    CreationDate = DateTimeOffset.Now,
                    EventDate = DateTimeOffset.Now.AddMinutes(-5),
                    FromState = "State1",
                    ToState = "State2",
                }
                .AddActions
                (
                     new PushedAction
                     {
                         Uuid = Guid.NewGuid(),
                         Name = Constants.PushReminder,
                         Kind = Constants.PushActionName,
                         Change = ChangeEnum.New,
                         ExecuteMessage = new MessageRaw()
                         {
                             Header = new MessageHeader(null),
                             Body = (MessageBlock)new DynObject()
                                .Add("name", "callMethod1")
                                .Add("arguments", new DynObject()
                                    .Add("arg1", "a1")
                                    .Add("arg2", "a2")
                                    ).Resolve(null),
                         }
                     }
                )
            );

            store.Save(w);

            var items = store.LoadByExternalId(w.ExternalId).First();
            
        }

    }


}
