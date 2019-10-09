# Workflow [![Build status](https://ci.appveyor.com/api/projects/status/r58r4d3rl4o60ohj?svg=true)](https://ci.appveyor.com/project/gaelgael5/calendarium-0v0lc)
workflow process with a language dedicated to functional business

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


			var processor = new WorkflowProcessor(config)
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
