using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Bb.Workflows.Models;

namespace Bb.Workflows.Converters
{

    public class WorkflowIncomingEventConverter : JsonConverter<IncomingEvent>
    {

        public override IncomingEvent ReadJson(JsonReader reader, Type objectType, IncomingEvent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {

            JValue v;
            JObject jObject = JObject.Load(reader);

            var result = new IncomingEvent()
            {
            };

            foreach (var item in jObject)
            {
                switch (item.Key)
                {

                    case "Uuid":
                        v = (JValue)item.Value;
                        result.Uuid = Guid.Parse(v.Value.ToString());
                        break;

                    case "ExternalId":
                        v = (JValue)item.Value;
                        result.ExternalId = v.Value.ToString();
                        break;

                    case "CreationDate":
                        v = (JValue)item.Value;
                        result.CreationDate = DateTimeOffset.Parse(v.Value.ToString());
                        break;

                    case "EventDate":
                        v = (JValue)item.Value;
                        result.EventDate = DateTimeOffset.Parse(v.Value.ToString());
                        break;

                    case "Name":
                        v = (JValue)item.Value;
                        result.Name = v.Value.ToString();
                        break;

                    default:

                        var o = new DynObject() { };
                        result.ExtendedDatas.Items.Add(item.Key, o);

                        if (item.Value is JValue v1)
                            o.SetValue(v1.Value.ToString());

                        else if (item.Value is JObject v2)
                            Deserialize(v2, o);

                        else if (item.Value is JArray v3)
                            Deserialize(v3, o);

                        else
                        {

                        }
                        break;
                }
            }

            return result;

        }

        private void Deserialize(JArray v, DynObject o)
        {

            o.IsArray = true;

            foreach (var item in v)
            {

                var j = new DynObject() { };
                o.Items.Add(o.Items.Count.ToString(), j);

                if (item is JValue v1)
                    j.SetValue(v1.Value.ToString());

                else if (item is JObject v2)
                    Deserialize(v2, j);

                else if (item is JArray v3)
                    Deserialize(v3, j);

            }
        }

        private void Deserialize(JObject v, DynObject o)
        {

            foreach (var item in v)
            {

                var j = new DynObject() { };
                o.Items.Add(item.Key, j);

                if (item.Value is JValue v1)
                    j.SetValue(v1.Value.ToString());

                else if (item.Value is JObject v2)
                    Deserialize(v2, o);

                else if (item.Value is JArray v3)
                    Deserialize(v3, o);

                else
                {

                }

            }

        }

        public override void WriteJson(JsonWriter writer, IncomingEvent value, JsonSerializer serializer)
        {

            JObject o = new JObject
            {
                new JProperty(nameof(value.Uuid), new JValue(value.Uuid)),
                new JProperty(nameof(value.ExternalId), new JValue(value.ExternalId)),
                new JProperty(nameof(value.CreationDate), new JValue(value.CreationDate)),
                new JProperty(nameof(value.EventDate), new JValue(value.EventDate)),
                new JProperty(nameof(value.Name), new JValue(value.Name)),

            };

            foreach (var item in value.ExtendedDatas.Items)
            {
                var r = item.Value.Serialize();
                if (r != null)
                    o.Add(new JProperty(item.Key, r));
            }

            o.WriteTo(writer);

        }

    }

}
