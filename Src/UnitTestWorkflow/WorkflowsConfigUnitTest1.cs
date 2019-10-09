using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Bb.Workflows;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Configurations;

namespace UnitTestWorkflow
{

    [TestClass]
    public class WorkflowsConfigUnitTest1
    {

        [TestMethod]
        public void GetConfigByName()
        {

            var config = new WorkflowsConfig();

            config.AddDocument(new WorkflowConfig()
            {
                Name = "wrk1",
                Label = "wrk1 config",
                Version = 1,
            }
            .AddFilter("Country", "France")
            );

            var c = config.Get("wrk1").Single();

            Assert.AreEqual(c.Name, "wrk1");

        }

        [TestMethod]
        public void GetConfigByNameAndVersion()
        {

            var config = new WorkflowsConfig();

            config.AddDocument(new WorkflowConfig()
            {
                Name = "wrk1",
                Label = "wrk1 config",
                Version = 1,
            }
            .AddFilter("Country", "France")
            );

            config.AddDocument(new WorkflowConfig()
            {
                Name = "wrk1",
                Label = "wrk1 config",
                Version = 2,
            }
            .AddFilter("Country", "France")
            );

            Assert.AreEqual(config.Get("wrk1", 2).Version, 2);
            Assert.AreEqual(config.Get("wrk1", 1).Version, 1);

        }

        [TestMethod]
        public void GetConfigEventIncoming()
        {

            var config = new WorkflowsConfig();

            config.AddDocument(new WorkflowConfig()
            {
                Name = "wrk1",
                Label = "wrk1 config",
                Version = 1,
            }
            .AddFilter("Country", "France")
            );

            config.AddDocument(new WorkflowConfig()
            {
                Name = "wrk2",
                Label = "wrk2 config",
                Version = 1,
            }
            .AddFilter("Country", "Germany")
            );

            var ev = new IncomingEvent()
            {
                Name = "evnt1",
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                EventDate = DateTimeOffset.Now,
                CreationDate = DateTimeOffset.Now,
            }
            .AddExtendedDatas("Country", "Germany");

            var item = config.Get(ev).First();

            Assert.AreEqual(item.Name, "wrk2");

        }

    }

}
