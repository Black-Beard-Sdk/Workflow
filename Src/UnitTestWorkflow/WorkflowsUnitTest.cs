using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Bb.Workflows;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Configurations;
using Bb.Workflows.Outputs;

namespace UnitTestWorkflow
{

    [TestClass]
    public partial class WorkflowsUnitTest
    {

        [TestMethod]
        public void InitializeWorkflow()
        {

            var config = new WorkflowsConfig();
            var storage = new MemoryStorage();

            config.AddDocument(
                new WorkflowConfig() { Name = "wrk1", Label = "wrk1 config", Version = 1, }

                .AddInitializer(new InitializationOnEventConfig() { EventName = "evnt1", }.AddSwitch("state1"))

                .AddState(new StateConfig() { Name = "State1", Label = "State1", }
                    .AddEvent(new IncomingEventConfig() { Name = "evnt1", }
                        .AddTransition(new TransitionConfig() { TargetStateName = "State2" })
                    )
                )
                .AddState(new StateConfig() { Name = "State2", Label = "State2" }
                )
            );


            var processor = new WorkflowProcessor(config)
            {
                OutputActions = () => CreateOutput(new JsonWorkflowSerializer(), storage)
            };


            var ev = new IncomingEvent()
            {
                Name = "evnt1",
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            };

            processor.EvaluateEvent(ev);

            var wrk = storage.GetAll<Workflow>().First(c => c.ExternalId == ev.ExternalId);

            Assert.AreEqual(wrk.CurrentState, "State2");

        }

        [TestMethod]
        public void InitializeWorkflowWithRule()
        {

            var config = new WorkflowsConfig();
            var storage = new MemoryStorage();

            config.AddDocument(
                new WorkflowConfig() { Name = "wrk1", Label = "wrk1 config", Version = 1, }

                .AddInitializer(new InitializationOnEventConfig() { EventName = "evnt1", }.AddSwitch("state1"))

                .AddState(new StateConfig() { Name = "State1", Label = "State1", }
                    .AddEvent(new IncomingEventConfig() { Name = "evnt2" }
                        .AddTransition(new TransitionConfig() { TargetStateName = "State2", WhenRule = (c) => c.IncomingEvent.Name == "evnt2" })
                    )
                )
                .AddState(new StateConfig() { Name = "State2", Label = "State2" }
                )
            );


            var processor = new WorkflowProcessor(config)
            {
                LoadExistingWorkflows = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
                OutputActions = () => CreateOutput(new JsonWorkflowSerializer(), storage)
            };


            var ev = new IncomingEvent()
            {
                Name = "evnt1",
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            };
            processor.EvaluateEvent(ev);
            var wrk = storage.GetAll<Workflow>().FirstOrDefault(c => c.ExternalId == ev.ExternalId);
            Assert.AreEqual(wrk, null);


            ev = new IncomingEvent()
            {
                Name = "evnt2",
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            };
            processor.EvaluateEvent(ev);
            wrk = storage.GetAll<Workflow>().FirstOrDefault(c => c.ExternalId == ev.ExternalId);
            Assert.AreEqual(wrk.WorkflowName, "wrk1");


        }

        [TestMethod]
        public void InitializeWorkflowWithPushedAction()
        {

            var config = new WorkflowsConfig();
            var storage = new MemoryStorage();

            config.AddDocument(
                new WorkflowConfig() { Name = "wrk1", Label = "wrk1 config", Version = 1, }

                .AddInitializer(new InitializationOnEventConfig() { EventName = "evnt1", }.AddSwitch("state1"))

                .AddState(new StateConfig() { Name = "State1", Label = "State1", }
                    .AddIncomingActions(null, new ResultActionConfig() { Name = "act_on_state_in_1" })
                    .AddOutcomingActions(null, new ResultActionConfig() { Name = "act_on_state_out" })
                    .AddEvent(new IncomingEventConfig() { Name = "evnt1" }
                    .AddAction(null, new ResultActionConfig() { Name = "act_on_event" })
                        .AddTransition(new TransitionConfig() { TargetStateName = "State2" }
                            .AddAction(null, new ResultActionConfig() { Name = "act_on_transition" })
                        )

                    )
                )
                .AddState(new StateConfig() { Name = "State2", Label = "State2" }
                    .AddIncomingActions(null, new ResultActionConfig() { Name = "act_on_state_in_2" }
                        .AddArgument("name", "@Event.Name")
                    )

                )
            );


            var processor = new WorkflowProcessor(config)
            {
                LoadExistingWorkflows = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
                OutputActions = () => CreateOutput(new JsonWorkflowSerializer(), storage),
            };

            var ev = new IncomingEvent()
            {
                Name = "evnt1",
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            };
            processor.EvaluateEvent(ev);
            var wrk = storage.GetAll<Workflow>().FirstOrDefault(c => c.ExternalId == ev.ExternalId);

            var act = wrk.LastEvent.Actions;

            act.First(c => c.Name == "act_on_state_in_1");
            act.First(c => c.Name == "act_on_state_out");
            act.First(c => c.Name == "act_on_event");
            act.First(c => c.Name == "act_on_transition");
            Assert.AreEqual( act.First(c => c.Name == "act_on_transition_2").Arguments["name"], "evnt1");

        }

        [TestMethod]
        public void SwitchState()
        {

            string externalId = Guid.NewGuid().ToString();
            var config = new WorkflowsConfig();
            var storage = new MemoryStorage();

            config.AddDocument(
                new WorkflowConfig() { Name = "wrk1", Label = "wrk1 config", Version = 1, }

                .AddInitializer(new InitializationOnEventConfig() {  EventName = "evnt1", }.AddSwitch("state1"))

                .AddState(new StateConfig() { Name = "State1", Label = "State1", }
                    .AddEvent(new IncomingEventConfig() { Name = "evnt1" }
                        .AddTransition(new TransitionConfig() { TargetStateName = "State2" })
                    )
                )

                .AddState(new StateConfig() { Name = "State2", Label = "State2" }
                    .AddOutcomingActions(null, new ResultActionConfig() { Name = "act1" })
                    .AddEvent(new IncomingEventConfig() { Name = "evnt2" }
                        .AddTransition(new TransitionConfig() { TargetStateName = "State3" })
                    )
                )

                .AddState(new StateConfig() { Name = "State3", Label = "State3" }
                    .AddIncomingActions(null, new ResultActionConfig() { Name = "act2" })

                )
            );


            var processor = new WorkflowProcessor(config)
            {
                LoadExistingWorkflows = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
                OutputActions = () => CreateOutput(new JsonWorkflowSerializer(), storage)
            };


            processor.EvaluateEvent(new IncomingEvent()
            {
                Name = "evnt1",
                Uuid = Guid.NewGuid(),
                ExternalId = externalId,
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            });
            var wrk = storage.GetAll<Workflow>().FirstOrDefault(c => c.ExternalId == externalId);
            Assert.AreEqual(wrk.CurrentState, "State2");


            processor.EvaluateEvent(new IncomingEvent()
            {
                Name = "evnt2",
                Uuid = Guid.NewGuid(),
                ExternalId = externalId,
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            });
            wrk = storage.GetAll<Workflow>().FirstOrDefault(c => c.ExternalId == externalId);
            Assert.AreEqual(wrk.CurrentState, "State3");
            wrk.LastEvent.Actions.First(c => c.Name == "act1");
            wrk.LastEvent.Actions.First(c => c.Name == "act2");


        }

        [TestMethod]
        public void AvaluateArgumentConstant()
        {
            var value = Guid.NewGuid().ToString();
            RunContext ctx = CreateContextForEvaluateArgument();
            var result = new ResultActionConfig() { Name = "act_on_state_in_1" }
            .AddArgument("v1", "toto")
            .Map(ctx);
            var v1 = result.Arguments.FirstOrDefault();
            Assert.AreEqual(v1.Value, "toto");
        }

        [TestMethod]
        public void EvaluateArgumentInEvent()
        {
            var _value = Guid.NewGuid().ToString();
            RunContext ctx = CreateContextForEvaluateArgument();
            var u = ExpressionHelper.GetAccessors(typeof(RunContext), "Event.ExternalId");
            var v = u(ctx);
            Assert.AreEqual(v, ctx.Event.ExternalId);
        }

        [TestMethod]
        public void EvaluateArgumentInDynamicObject()
        {
            var _value = Guid.NewGuid().ToString();
            RunContext ctx = CreateContextForEvaluateArgument();
            ctx.Event.ExtendedDatas.Items.Add("SiteIdentifier", new DynamicObject() { Value = "112233" });
            var u = ExpressionHelper.GetAccessors(typeof(RunContext), "Event.SiteIdentifier");
            var v = u(ctx);
            Assert.AreEqual(v, "112233");
        }

        private static RunContext CreateContextForEvaluateArgument()
        {
            var ev = new IncomingEvent()
            {
                Name = "evnt1",
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            };

            var wrk = new Workflow()
            {
                Uuid = Guid.NewGuid(),
                WorkflowName = "Test",
                Version = 1,
                ExternalId = ev.ExternalId,
                CreationDate = WorkflowClock.Now(),
                LastUpdateDate = WorkflowClock.Now(),
            };

            RunContext ctx = new RunContext(wrk, ev);

            return ctx;

        }

        public OutputAction CreateOutput(IWorkflowSerializer serializer, MemoryStorage storage)
        {

            return  new SetPropertiesOutputAction(
                        new PushBusActionOutputActionInMemory(storage,
                            new PushModelOutputActionInMemory(storage)
                        )
                        {
                            Serializer = serializer,
                        }
                    );

        }

    }

}
