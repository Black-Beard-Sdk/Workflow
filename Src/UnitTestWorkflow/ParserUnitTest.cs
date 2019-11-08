using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Bb.Workflows;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using Bb.Workflows.Parser;
using Bb.Workflows.Models.Configurations;
using Bb.Workflows.Outputs;
using System.Text;
using Bb.Workflows.Templates;
using Bb.Workflows.Models.Logs;

namespace UnitTestWorkflow
{

    [TestClass]
    public class ParserUnitTest
    {

        [TestMethod]
        public void TestUnserializeConfig()
        {

            var config = GetConfig(payload);

            Assert.AreEqual(config.Version, 2);
            Assert.AreEqual(config.Concurrency, 1);
            Assert.AreEqual(config.DeclaredEvents.Count, 4);

            Assert.AreEqual(config.Initializers.Count, 1);

            var i1 = config.Initializers["Event1"];

            Assert.AreEqual(config.States.Count, 3);

            var s1 = config.States["State1"];
            var s2 = config.States["State2"];
            var s3 = config.States["State3"];

            Assert.AreEqual(s1.Events.Count, 2);


            Assert.AreEqual(s1.Events["Event2"].Transitions.Count, 1);
            Assert.AreEqual(s1.Events["Event2"].Transitions[0].TargetStateName, "State2");
            Assert.AreEqual(s1.Events["Event2"].Transitions[0].TargetState, s2);

            Assert.AreEqual(s1.Events[Bb.Workflows.Constants.Events.ExpiredEventName].Transitions.Count, 1);
            Assert.AreEqual(s1.Events[Bb.Workflows.Constants.Events.ExpiredEventName].Transitions[0].TargetState, s3);

            Assert.AreEqual(s1.IncomingRules.Count, 2);
            Assert.AreNotEqual(s1.IncomingRules[0].WhenRule, null);
            Assert.AreEqual(s1.IncomingRules[0].Actions.Count, 2);
            Assert.AreEqual(s1.IncomingRules[0].Actions[0].Kind, "$_push");
            Assert.AreEqual(s1.IncomingRules[0].Actions[1].Kind, "$_update");


            Assert.AreEqual(s1.OutcomingRules.Count, 1);
            Assert.AreNotEqual(s1.OutcomingRules[0].WhenRule, null);
            Assert.AreEqual(s1.OutcomingRules[0].Actions.Count, 1);
            Assert.AreEqual(s1.OutcomingRules[0].Actions[0].Kind, "$_push");


        }


        [TestMethod]
        public void TestFilter()
        {

            string txt;
            var storage = new MemoryStorage();
            var engine = CreateEngine(storage, payload);

            // not integrated because no country specified
            txt = MessageText
                .Text("Name", "Event1")
                .Add("Uuid", Guid.NewGuid())
                .Add("ExternalId", Guid.NewGuid())
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5));
            engine.EvaluateEvent(txt);
            Assert.AreEqual(storage.GetAll<Workflow>().Count(), 0);

            // not integrated because country specified is wrong
            txt = MessageText
                .Text("Name", "Event1")
                .Add("Uuid", Guid.NewGuid())
                .Add("ExternalId", Guid.NewGuid())
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("Country", "Germany");
            engine.EvaluateEvent(txt);
            Assert.AreEqual(storage.GetAll<Workflow>().Count(), 0);

            // not integrated because event name is bypassed
            txt = MessageText
                .Text("Name", "Event666")
                .Add("Uuid", Guid.NewGuid())
                .Add("ExternalId", Guid.NewGuid())
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("Country", "Germany");
            engine.EvaluateEvent(txt);
            Assert.AreEqual(storage.GetAll<Workflow>().Count(), 0);

            // must be integrated
            txt = MessageText
                .Text("Name", "Event1")
                .Add("Uuid", Guid.NewGuid())
                .Add("ExternalId", Guid.NewGuid())
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("Country", "France");
            engine.EvaluateEvent(txt);
            Assert.AreEqual(storage.GetAll<Workflow>().Count(), 1);

        }


        [TestMethod]
        public void TestInitializeWithJumpState()
        {

            string txt;
            var storage = new MemoryStorage();
            var engine = CreateEngine(storage, payload2);

            // must be integrated
            txt = MessageText
                .Text("Name", "Event1")
                .Add("Uuid", Guid.NewGuid())
                .Add("ExternalId", Guid.NewGuid())
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("Country", "France")
                .Add("Age", 25);
            engine.EvaluateEvent(txt);

            var workF = storage.GetAll<Workflow>().FirstOrDefault();

            Assert.AreEqual(workF.CurrentState, "State2");

        }


        [TestMethod]
        public void TestInitialize()
        {

            string txt;
            string uuid = Guid.NewGuid().ToString();
            var storage = new MemoryStorage();
            var engine = CreateEngine(storage, payload);

            // must be integrated
            txt = MessageText
                .Text("Name", "Event1")
                .Add("Uuid", Guid.NewGuid())
                .Add("ExternalId", uuid)
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("Country", "France")
                .Add("age", 25)
                ;
            engine.EvaluateEvent(txt);

            var workF = storage.GetAll<Workflow>().FirstOrDefault();
            Assert.AreEqual(workF.CurrentState, "State1");


            // must be integrated
            txt = MessageText
                .Text("Name", "Event2")
                .Add("Uuid", uuid)
                .Add("ExternalId", uuid)
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("Country", "France")
                .Add("age", 25)
                ;
            engine.EvaluateEvent(txt);
            workF = storage.GetAll<Workflow>().FirstOrDefault();
            Assert.AreEqual(workF.CurrentState, "State2");

        }

        [TestMethod]
        public void TestExpire()
        {

            string txt;
            string uuid = Guid.NewGuid().ToString();
            var storage = new MemoryStorage();
            var engine = CreateEngine(storage, payload);

            txt = MessageText
                .Text("Name", "Event1")
                .Add("Uuid", Guid.NewGuid())
                .Add("ExternalId", uuid)
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("Country", "France")
                .Add("age", 25)
                ;
            engine.EvaluateEvent(txt);

            var wor = storage.GetAll<Workflow>().FirstOrDefault();
            var bus = storage.GetAll<PushedAction>().ToList();
            Assert.AreEqual(wor.CurrentState, "State1");


            // must be integrated
            txt = MessageText
                .Text("Name", Constants.Events.ExpiredEventName)
                .Add("Uuid", uuid)
                .Add("ExternalId", uuid)
                .Add("WorkflowId", wor.Uuid)
                .Add("CreationDate", WorkflowClock.Now())
                .Add("EventDate", WorkflowClock.Now().AddMinutes(-5))
                .Add("CurrentState", "State1");
            engine.EvaluateEvent(txt);
            wor = storage.GetAll<Workflow>().FirstOrDefault();
            
            Assert.AreEqual(wor.CurrentState, "State3");

        }

        private WorkflowEngine CreateEngine(MemoryStorage storage, string configText)
        {

            var template = new TemplateRepository(typeof(TemplateModels))
            {
                DefaultAction = TemplateModels.DefaultAction,
            };
            var metadatas = new MetadatRepository(typeof(MetadataModels))
            {
                DefaultAction = MetadataModels.DefaultAction.ToDictionary(c => c.Key, c => c.Value),
            };

            var serializer = new PartialJsonWorkflowSerializer();

            WorkflowsConfig configs = new WorkflowsConfig()
                .AddDocument(GetConfig(configText))
                ;

            var factory = new WorkflowFactory<RunContext>(null, null)
            {
                Serializer = serializer,
            };

            var processor = new WorkflowProcessor<RunContext>(configs, factory)
            {
                LoadExistingWorkflowsByExternalId = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
                OutputActions = () => CreateOutput(serializer, storage),
                Templates = template,
                Metadatas = metadatas,
            };

            WorkflowEngine engine = new WorkflowEngine()
            {
                Processor = processor,
            };

            return engine;

        }

        public OutputAction CreateOutput(IWorkflowSerializer serializer, MemoryStorage storage)
        {

            return new SetPropertiesOutputAction(
                        new PushBusActionOutputActionInMemory(storage,
                            new PushModelOutputActionInMemory(storage)
                        )
                    );

        }

        private static WorkflowConfig GetConfig(string payload)
        {

            var tree = WorkflowConfigParser.ParseString(new System.Text.StringBuilder(payload));

            WorkflowConfigVisitor visitor = new WorkflowConfigVisitor()
            .AddRule("IsMajor", typeof(ParserUnitTest).GetMethod("IsMajor"))
            .AddRule("IsEmpty", typeof(ParserUnitTest).GetMethod("IsEmpty"))
            ;

            visitor.Filename = "memory text";
            WorkflowConfig config = (WorkflowConfig)tree.Visit<object>(visitor);

            return config;

        }

       
        public static bool IsMajor(RunContext ctx, int agemin)
        {
            var p = ctx.IncomingEvent.ExtendedDatas()["Age"];
            if (p == null || p == DynObject.None)
                return false;
            var value = p.ValueAs<int>(ctx);
            return value >= agemin;
        }

        public static bool IsEmpty(RunContext ctx, string text)
        {
            return string.IsNullOrEmpty((string)text);
        }




        private string payload = @"
    NAME wrk1 VERSION 2
    CONCURENCY 1
    DESCRIPTION                 'workflow de test'
    MATCHING (Country = 'France')

    DEFINE EVENT     Event1                     'incoming event 1';
    DEFINE EVENT     Event2                     'incoming event 2';

    DEFINE RULE      IsMajor (INTEGER agemin)   'this method 1';
    DEFINE RULE      IsEmpty (TEXT text)        'this method 2'; 

    DEFINE ACTION    Cut(TEXT key)              'Remove user';

    DEFINE CONST     Name 'gael'                'ben oui c est moi';
    DEFINE CONST     agemin 18                  'min for been major';

    INITIALIZE WORKFLOW
        ON EVENT Event1 WHEN NOT IsEmpty(text = @Workflow.ExternalId) 
            SWITCH State1

    DEFINE STATE State1                         'state 1'
        ON ENTER STATE 
            WHEN IsMajor(agemin = agemin)
                EXECUTE Cut(key = @Workflow.ExternalId)
                     -- Cut(key = @Workflow.ExternalId)
                STORE   (Status = 'InProgress')           
                     -- (Status = 'InProgress')           

        ON EVENT Event2
            SWITCH State2 

        EXPIRE AFTER 2 DAY
            SWITCH State3

    ;

    DEFINE STATE State2            'state 2'

    ;

    DEFINE STATE State3            'state 3'
    
    ;

";

        private string payload2 = @"
    NAME wrk1 VERSION 2
    CONCURENCY 1
    DESCRIPTION                 'workflow de test'
    MATCHING (Country = 'France')

    DEFINE EVENT     Event1                     'incoming event 1';
    DEFINE EVENT     Event2                     'incoming event 2';

    DEFINE RULE      IsMajor (INTEGER agemin)   'this method 1';
    DEFINE RULE      IsEmpty (TEXT text)        'this method 2'; 

    DEFINE ACTION    Cut(TEXT key)              'Remove user';

    DEFINE CONST     Name 'gael'                'ben oui c est moi';
    DEFINE CONST     agemin 18                  'min for been major';

    INITIALIZE WORKFLOW
        ON EVENT Event1 WHEN (NOT IsEmpty(text = @Workflow.ExternalId)) 
            RECURSIVE SWITCH State1

    DEFINE STATE State1                         'state 1'
        ON ENTER STATE 
            WHEN IsMajor(agemin = agemin)
                EXECUTE Cut(key = @Workflow.ExternalId)
                     -- Cut(key = @Workflow.ExternalId)
                STORE   (Status = 'InProgress')           
                     -- (Status = 'InProgress')           

        ON EVENT Event1
            SWITCH State2 

        EXPIRE AFTER 2 DAY
            SWITCH State3

    ;

    DEFINE STATE State2            'state 2'

    ;

    DEFINE STATE State3            'state 3'
    
    ;

";

    }

}

