using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Bb.Workflows;
using Bb.Workflows.Converters;
using Bb.Workflows.Models;
using Bb.Workflows.Parser;
using Bb.Workflows.Models.Configurations;

namespace UnitTestWorkflow
{

    [TestClass]
    public class ParserUnitTest
    {

        [TestMethod]
        public void UnserializeEventIncoming()
        {

            string payload = @"
    NAME wrk1 VERSION 2
    CONCURENCY 1
    DESCRIPTION                 'workflow de test'
    MATCHING (Country = 'France')

    DEFINE EVENT     Event1                     'incoming event 1';
    DEFINE EVENT     Event2                     'incoming event 2';

    DEFINE RULE      IsMajor (INTEGER agemin)   'this method 1';
    DEFINE RULE      IsEmpty (TEXT name)        'this method 2'; 

    DEFINE ACTION    Cut(TEXT key)              'Remove user';

    DEFINE CONST     Name 'gael'                'ben oui c est moi';
    DEFINE CONST     agemin 18                  'min for been major';

    INITIALIZE WORKFLOW
        ON EVENT Event1 WHEN !IsEmpty(name = @Event.ExternalId) 
            SWITCH State1

    DEFINE STATE State1                         'state 1'
        ON ENTER STATE 
            WHEN !IsMajor(agemin = agemin)
                EXECUTE Cut(key = @Event.ExternalId)
                     -- Cut(key = @Event.ExternalId)
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

            var config = GetConfig(payload);

            Assert.AreEqual(config.Version, 2);
            Assert.AreEqual(config.Concurrency, 1);
            Assert.AreEqual(config.DeclaredEvents.Count, 3);

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
            Assert.AreNotEqual(s1.IncomingRules[0].Rule, null);
            Assert.AreEqual(s1.IncomingRules[0].Actions.Count, 2);
            Assert.AreEqual(s1.IncomingRules[0].Actions[0].Kind, "$_push");
            Assert.AreEqual(s1.IncomingRules[0].Actions[1].Kind, "$_update");


            Assert.AreEqual(s1.OutcomingRules.Count, 1);
            Assert.AreNotEqual(s1.OutcomingRules[0].Rule, null);
            Assert.AreEqual(s1.OutcomingRules[0].Actions.Count, 1);
            Assert.AreEqual(s1.OutcomingRules[0].Actions[0].Kind, "$_push");


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
            return ctx.IncomingEvent.ExtendedDatas["age"].ValueAs<int>() >= agemin;
        }

        public static bool IsEmpty(RunContext ctx)
        {
            return string.IsNullOrEmpty((string)ctx.Arguments["name"]);
        }


    }

}

