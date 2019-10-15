using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bb.Workflows.Models;

namespace Bb.Workflows.Converters
{

    public class WorkflowConverter : JsonConverter<Workflow>
    {

        public override Workflow ReadJson(JsonReader reader, Type objectType, Workflow existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, Workflow value, JsonSerializer serializer)
        {

            JObject o = new JObject
            {
                new JProperty(nameof(value.Uuid), new JValue(value.Uuid)),
                new JProperty(nameof(value.ExternalId), new JValue(value.ExternalId)),
                new JProperty(nameof(value.CreationDate), new JValue(value.CreationDate)),
                new JProperty(nameof(value.LastUpdateDate), new JValue(value.LastUpdateDate)),
                new JProperty(nameof(value.Version), new JValue(value.Version)),
                new JProperty(nameof(value.WorkflowName), new JValue(value.WorkflowName))
            };

            foreach (var item in value.ExtendedDatas.Items)
            {
                var v = item.Value.Serialize();
                if (v != null)
                    o.Add(new JProperty(item.Key, v));
            }

            JArray a2 = new JArray();
            foreach (Event e in value.Events)
            {
                var v = Serialize(e);
                a2.Add(v);
            }
            o.Add(new JProperty(nameof(value.Events), a2));

            o.WriteTo(writer);

        }

        private JObject Serialize(Event value)
        {

            JObject result = new JObject
            {
                new JProperty(nameof(value.Name), new JValue(value.Name)),
                new JProperty(nameof(value.Uuid), new JValue(value.Uuid)),
                new JProperty(nameof(value.ExternalId), new JValue(value.ExternalId)),
                new JProperty(nameof(value.CreationDate), new JValue(value.CreationDate)),
                new JProperty(nameof(value.EventDate), new JValue(value.EventDate)),
                new JProperty(nameof(value.FromState), new JValue(value.FromState)),
                new JProperty(nameof(value.ToState), new JValue(value.ToState)),
            };

            foreach (var item in value.ExtendedDatas.Items)
            {
                var r = item.Value.Serialize();
                if (r != null)
                    result.Add(new JProperty(item.Key, r));
            }

            JArray a = new JArray();


            foreach (PushedAction item in value.Actions)
            {

                JObject push = new JObject
                {
                    new JProperty(nameof(item.Name), new JValue(value.Name)),
                    new JProperty(nameof(item.Uuid), new JValue(value.Uuid)),
                };

                if (item.ExecuteMessage != null)
                    new JProperty(nameof(item.ExecuteMessage), item.ExecuteMessage.Serialize());

                if (item.ResultMessage != null)
                    new JProperty(nameof(item.ResultMessage), item.ResultMessage.Serialize());

                if (item.CancelMessage != null)
                    new JProperty(nameof(item.CancelMessage), item.CancelMessage.Serialize());

                if (item.ResultCancelMessage != null)
                    new JProperty(nameof(item.ResultCancelMessage), item.ResultCancelMessage.Serialize());

                a.Add(push);

            }

            result.Add(new JProperty(nameof(value.Actions), a));

            return result;

        }

    }


}
