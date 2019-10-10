# Workflow [![Build status](https://ci.appveyor.com/api/projects/status/r58r4d3rl4o60ohj?svg=true)](https://ci.appveyor.com/project/gaelgael5/calendarium-0v0lc)
workflow process with a language dedicated to functional business

Syntaxe for write valid configuration
```Antlr
script :
    script_fragment | script_full
    EOF
    ;

script_fragment :
    FRAGMENT NAME key 
    (DESCRIPTION comment)?
	(define_referenciel_statement SEMICOLON)+ 
    ;

script_full :
    (INCLUDE CHAR_STRING)*
    NAME key (VERSION versionNumber=number)?
    concurency?
    (DESCRIPTION comment)?
    (MATCHING matchings)? 
	(define_referenciel_statement SEMICOLON)+ 
    initializing?
	(define_state_statement SEMICOLON)*
    ;

concurency :
    (CONCURENCY concurencyNumber=number)
    ;

define_referenciel_statement :
	DEFINE
    (  
        event_declaration_statement
      | rule_declaration_statement
      | action_declaration_statement
      | constant_declaration
    )    
    ;

define_state_statement : 
    DEFINE state
    ;

constant_declaration :
    CONST key value comment?
    ;

value :
    string | number | delay | REGULAR_ID
    ;

state :
    STATE key comment?
    execute*
    on_event_statement*
    ;

initializing : 
    INITIALIZE WORKFLOW initializing_item+
    ;

initializing_item :
    ON EVENT key (WHEN rule_conditions)? SWITCH key
    ;

on_event_statement :
    (ON EVENT key | EXPIRE AFTER delay) switch_state+
    ;    

switch_state :
    (WHEN rule_conditions)?
    (
          execute2* SWITCH key
        | execute2+
    )
    ;

execute :
    ON (ENTER STATE | EXIT STATE | ENTER AND EXIT STATE) 
    execute2
    ;

execute2 :
    (WHEN rule_conditions)? 
    (WAITING delay BEFORE)? 
    execute3+
    ;

execute3 :
      (EXECUTE | key) action+
    | STORE matchings+
    ;

matchings : 
    LEFT_PAREN matching+ RIGHT_PAREN
    ;

matching :
    key EQUAL string
    ;

action : 
    key LEFT_PAREN arguments? RIGHT_PAREN
    ;
    
arguments : 
    argument (COMMA argument)*
    ;

argument :
    key EQUAL argumentValue
    ;

argumentValue :
      string 
    | compositekey
    ;

rule_conditions :
      key LEFT_PAREN arguments? RIGHT_PAREN
    | NOT rule_conditions
    | rule_conditions AND rule_conditions
    | rule_conditions OR rule_conditions
    | LEFT_PAREN rule_conditions RIGHT_PAREN
    ;

event_declaration_statement :
    EVENT key comment?
    ;

action_declaration_statement :
    ACTION key LEFT_PAREN parameters? RIGHT_PAREN comment?
    ;

rule_declaration_statement :
    RULE key LEFT_PAREN parameters? RIGHT_PAREN comment?
    ;

parameters : 
    parameter (COMMA parameter)*
    ;

parameter :
    type key
    ;

type : 
    TEXT | INTEGER | DECIMAL
    ;

key : REGULAR_ID;

compositekey : 
    AROBASE? key (DOT key)
    ;

comment : CHAR_STRING;

number : NUMBER;

numeric :
    number DOT number
    ;

string :
	CHAR_STRING
    ;

delay :
    number (MINUTE | HOUR | DAY)
    ;




```

Exemple de configuration workflow
```Configuration
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
        ON EVENT Event1 WHEN NOT IsEmpty(name = @Event.ExternalId) 
            SWITCH State1

    DEFINE STATE State1                         'state 1'
        ON ENTER STATE 
            WHEN NOT IsMajor(agemin = agemin)
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
```

## How to Use
```CSharp

	// Initialize

	// momery starage for unit tests
	var storage = new MemoryStorage();
	
	// parse configuration
	var tree = WorkflowConfigParser.ParseString(new System.Text.StringBuilder(payload));
	
	WorkflowConfigVisitor visitor = new WorkflowConfigVisitor()
	  .AddRule("IsMajor", typeof(FunctionalRules).GetMethod("IsMajor")) -- add custom rule function
	  .AddRule("IsEmpty", typeof(FunctionalRules).GetMethod("IsEmpty"))-- add custom rule function
	;
	
	visitor.Filename = "memory text";
	WorkflowConfig config = (WorkflowConfig)tree.Visit<object>(visitor);
	
	var configs = new WorkflowsConfig();
	configs.AddDocument(config);
	
	
	var processor = new WorkflowProcessor(configs)
	{
	    LoadExistingWorkflows = (key) => storage.GetBy<Workflow, string>(key, c => c.ExternalId).ToList(),
	    OutputActions = () => CreateOutput(new JsonWorkflowSerializer(), storage),
	};
	
	
	// create decorator of worker on the context
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

	// functional rules
	public static class FunctionalRules
	{
	
	    public static bool IsMajor(RunContext ctx, int agemin)
        {
            return ctx.IncomingEvent.ExtendedDatas["age"].ValueAs<int>() >= agemin;
        }

        public static bool IsEmpty(RunContext ctx)
        {
            return string.IsNullOrEmpty((string)ctx.Arguments["name"]);
        }

	}

	// Create


	// Deserialize incoming message

	string payload = @"{
      'Name': 'evnt1',
      'Uuid': '352d5082-3fb9-4db8-8a59-bc864ade4929',
      'ExternalId': 'c65ea000-dce0-44a3-a6f3-9daf7eebaefe',
      'CreationDate': '2019-09-23T09:48:52.1868472+02:00',
      'EventDate': '2019-09-23T09:48:47.3047926+02:00',   
      'Site': 'site1',
      'infos': { 'name': 'toto' },
      'infos2': [ { 'name': 'toto' }, { 'name': 'titi' } ],
    }".Replace("'", "\"");

	IWorkflowSerializer serializer = new JsonWorkflowSerializer();
    var msg = serializer.Unserialize(payload);

	// evaluate event for workflows
	processor.EvaluateEvent(ev);


// Get all dates for country france
int year = DateTime.Now.Year;
var dates = cal.GetDates(year, "France");
