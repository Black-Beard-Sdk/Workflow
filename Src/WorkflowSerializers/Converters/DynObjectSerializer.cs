using Newtonsoft.Json.Linq;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Messages;

namespace Bb.Workflows.Converters
{
    public static class DynObjectSerializer
    {

        public static JToken Serialize(this DynObject value)
        {

            JToken result = null;
            object v = value.GetValue != null ? value.GetValue(null) : null;
            if (v != null)
            {
                if (v is string s)
                {
                    if (!string.IsNullOrEmpty(s))
                        result = new JValue(v);
                }
                else
                    result = new JValue(v);
            }
            else
            {

                if (value.IsArray)
                {
                    var ar = new JArray();
                    foreach (var item in value.Items)
                    {
                        var r = Serialize(item.Value);
                        if (r != null)
                            ar.Add(r);
                    }
                    if (ar.Count > 0)
                        result = ar;
                }
                else
                {
                    var oj = new JObject();
                    foreach (var item in value.Items)
                    {
                        var r = Serialize(item.Value);
                        if (r != null)
                            oj.Add(new JProperty(nameof(item.Key), r));
                    }
                    if (oj.Count > 0)
                        result = oj;
                }

            }

            return result;

        }


        public static JToken Serialize(this MessageRaw self)
        {
            return visitor.VisitMessageRaw(self);
        }

        private static VisitorJsonSerialiser visitor = new VisitorJsonSerialiser();

    }

}



