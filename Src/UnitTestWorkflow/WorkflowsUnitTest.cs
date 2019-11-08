using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Bb.Workflows;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Configurations;
using Bb.Workflows.Outputs;
using Bb.Workflows.Templates;
using System.Reflection;
using Bb.Workflows.Expresssions;

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

                .AddInitializer(new InitializationOnEventConfig() { EventName = "evnt1", Recursive = true }.AddSwitch("State1"))

                .AddState(new StateConfig() { Name = "State1", Label = "State1", }
                    .AddEvent(new IncomingEventConfig() { Name = "evnt1", }
                        .AddTransition(new TransitionConfig() { TargetStateName = "State2" })
                    )
                )
                .AddState(new StateConfig() { Name = "State2", Label = "State2" }
                )
            );


            var template = new TemplateRepository(typeof(TemplateModels))
            {
                DefaultAction = TemplateModels.DefaultAction,
            };
            var metadatas = new MetadatRepository(typeof(MetadataModels))
            {
                DefaultAction = MetadataModels.DefaultAction.ToDictionary(c => c.Key, c => c.Value),
            };

            var factory = new WorkflowFactory<RunContext>(null, null)
            {
                Serializer = new PartialJsonWorkflowSerializer()
            };

            var processor = new WorkflowProcessor<RunContext>(config, factory)
            {
                OutputActions = () => CreateOutput(new PartialJsonWorkflowSerializer(), storage),
                Templates = template,
                Metadatas = metadatas,
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
                .AddInitializer(new InitializationOnEventConfig() { EventName = "evnt1", }.AddSwitch("State1"))
                .AddState(new StateConfig() { Name = "State1", Label = "State1", }
                    .AddEvent(new IncomingEventConfig() { Name = "evnt2" }
                        .AddTransition(new TransitionConfig() { TargetStateName = "State2", WhenRule = (c) => c.IncomingEvent.Name == "evnt2" })
                    )
                )
                .AddState(new StateConfig() { Name = "State2", Label = "State2" }
                )
            );

            var template = new TemplateRepository(typeof(TemplateModels))
            {
                DefaultAction = TemplateModels.DefaultAction,
            };
            var metadatas = new MetadatRepository(typeof(MetadataModels))
            {
                DefaultAction = MetadataModels.DefaultAction.ToDictionary(c => c.Key, c => c.Value),
            };

            var factory = new WorkflowFactory<RunContext>(null, null)
            {
                Serializer = new PartialJsonWorkflowSerializer()
            };

            var processor = new WorkflowProcessor<RunContext>(config, factory)
            {
                LoadExistingWorkflowsByExternalId = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
                OutputActions = () => CreateOutput(new PartialJsonWorkflowSerializer(), storage),
                Templates = template,
                Metadatas = metadatas,
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
            Assert.AreEqual(wrk.CurrentState, "State1");


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
            Assert.AreEqual(wrk, null);


        }

        [TestMethod]
        public void InitializeWorkflowWithPushedAction()
        {

            var config = new WorkflowsConfig();
            var storage = new MemoryStorage();

            config.AddDocument(
                new WorkflowConfig() { Name = "wrk1", Label = "wrk1 config", Version = 1, }

                .AddInitializer(new InitializationOnEventConfig() { EventName = "evnt1", Recursive = true }.AddSwitch("State1"))

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

            var template = new TemplateRepository(typeof(TemplateModels))
            {
                DefaultAction = TemplateModels.DefaultAction,
            };
            var metadatas = new MetadatRepository(typeof(MetadataModels))
            {
                DefaultAction = MetadataModels.DefaultAction.ToDictionary(c => c.Key, c => c.Value),
            };
            var factory = new WorkflowFactory<RunContext>(null, null)
            {
                Serializer = new PartialJsonWorkflowSerializer()
            };
            var processor = new WorkflowProcessor<RunContext>(config, factory)
            {
                LoadExistingWorkflowsByExternalId = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
                OutputActions = () => CreateOutput(new PartialJsonWorkflowSerializer(), storage),
                Templates = template,
                Metadatas = metadatas,
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

        }

        [TestMethod]
        public void SwitchState()
        {

            string externalId = Guid.NewGuid().ToString();
            var config = new WorkflowsConfig();
            var storage = new MemoryStorage();

            config.AddDocument(
                new WorkflowConfig() { Name = "wrk1", Label = "wrk1 config", Version = 1, }

                .AddInitializer(new InitializationOnEventConfig() { EventName = "evnt1", Recursive = true }.AddSwitch("State1"))

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

            var template = new TemplateRepository(typeof(TemplateModels))
            {
                DefaultAction = TemplateModels.DefaultAction,
            };
            var metadatas = new MetadatRepository(typeof(MetadataModels))
            {
                DefaultAction = MetadataModels.DefaultAction.ToDictionary(c => c.Key, c => c.Value)
            };
            var factory = new WorkflowFactory<RunContext>(null, null)
            {
                Serializer = new PartialJsonWorkflowSerializer()
            };
            var processor = new WorkflowProcessor<RunContext>(config, factory)
            {
                LoadExistingWorkflowsByExternalId = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
                OutputActions = () => CreateOutput(new PartialJsonWorkflowSerializer(), storage),
                Templates = template,
                Metadatas = metadatas,
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
            Assert.AreEqual(v1.Value.GetValue(ctx), "toto");
        }

        [TestMethod]
        public void EvaluateArgumentInEvent()
        {
            var _value = Guid.NewGuid().ToString();
            RunContext ctx = CreateContextForEvaluateArgument();
            var u = ExpressionDynobjectExtension.GetAccessors<RunContext>("Workflow.ExternalId");
            var v = u(ctx);
            Assert.AreEqual(v, ctx.Workflow.ExternalId);
        }

        [TestMethod]
        public void EvaluateArgumentInDynamicObject()
        {
            var _value = Guid.NewGuid().ToString();
            RunContext ctx = CreateContextForEvaluateArgument();
            ctx.Event.ExtendedDatas().Items.Add("SiteIdentifier", new DynObject().SetValue("112233"));
            var u = ExpressionDynobjectExtension.GetAccessors<RunContext>("Event.SiteIdentifier");
            var v = u(ctx);
            Assert.AreEqual(v, "112233");
        }

        private static RunContext CreateContextForEvaluateArgument()
        {

            var factory = new WorkflowFactory<RunContext>(null, null)
            {
                Serializer = new PartialJsonWorkflowSerializer()
            };

            var ev = new IncomingEvent()
            {
                Name = "evnt1",
                Uuid = Guid.NewGuid(),
                ExternalId = Guid.NewGuid().ToString(),
                EventDate = WorkflowClock.Now(),
                CreationDate = WorkflowClock.Now(),
            };

            var wrk = factory.CreateNewWorkflow(
                "Test",
                1,
                ev.ExternalId, 
                new DynObject()
            );

            RunContext ctx = new RunContext().Set(wrk, ev, factory);

            return ctx;

        }

        public OutputAction CreateOutput(IWorkflowSerializer serializer, MemoryStorage storage)
        {

            return new SetPropertiesOutputAction(
                        new PushBusActionOutputActionInMemory(storage,
                            new PushModelOutputActionInMemory(storage)
                        )
                   );

        }

    }


}
