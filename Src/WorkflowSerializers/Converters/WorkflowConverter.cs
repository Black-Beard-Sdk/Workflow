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
                o.Add(new JProperty(item.Key, Serialize(item.Value)));

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
                result.Add(new JProperty(item.Key, Serialize(item.Value)));

            JArray a = new JArray();
            foreach (var item in value.Actions)
                a.Add(Serialize(item));
            result.Add(new JProperty(nameof(value.Actions), a));

            return result;

        }

        private JToken Serialize(DynamicObject value)
        {

            JToken result;

            if (!string.IsNullOrEmpty(value.Value))
                result = new JValue(value.Value);

            else
            {

                if (value.IsArray)
                {
                    var ar = new JArray();
                    foreach (var item in value.Items)
                        ar.Add(Serialize(item.Value));
                    result = ar;
                }
                else
                {
                    var oj = new JObject();
                    foreach (var item in value.Items)
                        oj.Add(new JProperty(nameof(item.Key), Serialize(item.Value)));
                    result = oj;
                }

            }

            return result;

        }

        private JObject Serialize(ResultAction value)
        {

            JObject result = new JObject
            {
                new JProperty(nameof(value.Uuid), new JValue(value.Uuid)),
                new JProperty(nameof(value.EventDate), new JValue(value.EventDate)),
                new JProperty(nameof(value.Label), new JValue(value.Label)),
                new JProperty(nameof(value.Name), new JValue(value.Name)),
            };

            JArray j = new JArray();

            foreach (var item in value.Arguments)
                j.Add(new JProperty(nameof(item.Key), new JValue(item.Value)));

            result.Add(new JProperty("Arguments", j));

            return result;

        }
    }

}
