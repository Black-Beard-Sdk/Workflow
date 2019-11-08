using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Bb.Workflows;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;

namespace UnitTestWorkflow
{

    [TestClass]
    public class IncomingEventUnitTest
    {

        [TestMethod]
        public void SerializeOutputAction()
        {

            string uuid = Guid.NewGuid().ToString();

            DynObject.Create("Name", Constants.PushReminder)
            .Add("delay", WorkflowClock.Now().AddMinutes(10).ToString())
            .Add("canal", (ctx) => "IncomingEvent") // --read configuration where canal can be found
            .Add("Message",
                 DynObject.Create("Uuid", uuid)
                     .Add("Name", Constants.Events.ExpiredEventName)
                     .Add("EventDate", (ctx) => WorkflowClock.Now().AddMinutes(10).ToString())
                     .Add("CreationDate", (ctx) => WorkflowClock.Now().ToString())
                     .Add("ExternalId", (ctx) => ctx.Workflow.ExternalId)
                     .Add("CurrentState", (ctx) => ctx.Workflow.CurrentState))

            ;



        }

        [TestMethod]
        public void UnserializeEventIncoming()
        {

            string payload = @"{
      'Name': 'evnt1',
      'Uuid': '352d5082-3fb9-4db8-8a59-bc864ade4929',
      'ExternalId': 'c65ea000-dce0-44a3-a6f3-9daf7eebaefe',
      'CreationDate': '2019-09-23T09:48:52.1868472+02:00',
      'EventDate': '2019-09-23T09:48:47.3047926+02:00',   
      'Site': 'site1',
      'infos': { 'name': 'toto' },
      'infos2': [ { 'name': 'toto' }, { 'name': 'titi' } ]
    }".Replace("'", "\"");


            IWorkflowSerializer serializer = new PartialJsonWorkflowSerializer();
            var msg = new IncomingEvent();
            serializer.Populate(msg, payload);

            var payload2 = serializer.Serialize(msg);

            msg = new IncomingEvent();
            serializer.Populate(msg , payload2);
            var payload3 = serializer.Serialize(msg);

            Assert.AreEqual(payload2, payload3);

        }

    }

}
