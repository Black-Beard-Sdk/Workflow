using Newtonsoft.Json;
using Bb.Workflows.Models;

namespace Bb.Workflows.Converters
{


    public class JsonWorkflowSerializer : IWorkflowSerializer
    {


        public JsonWorkflowSerializer()
        {

            this._settings = new JsonSerializerSettings()
            {
                
            };

            this._settings.Converters.Add(new WorkflowIncomingEventConverter());
            this._settings.Converters.Add(new WorkflowConverter());

        }


        public string Serialize(Workflow workflow)
        {
            var json = JsonConvert.SerializeObject(workflow, Formatting.Indented, _settings);
            return json;
        }


        public string Serialize(PushedAction pushedAction)
        {
            var json = JsonConvert.SerializeObject(pushedAction, Formatting.Indented, _settings);
            return json;
        }



        public IncomingEvent Unserialize(string payload)
        {
            var result = JsonConvert.DeserializeObject<IncomingEvent>(payload, _settings);
            return result;
        }

        public string Serialize(IncomingEvent @event)
        {
            var result = JsonConvert.SerializeObject(@event, Formatting.Indented, _settings);
            return result;
        }


        private readonly JsonSerializerSettings _settings;

    }


}
