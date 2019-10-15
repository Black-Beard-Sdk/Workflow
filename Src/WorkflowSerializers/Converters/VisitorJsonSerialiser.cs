using Newtonsoft.Json.Linq;
using Bb.Workflows.Models.Messages;

namespace Bb.Workflows.Converters
{
    internal class VisitorJsonSerialiser : MessageVisitor<JToken>
    {


        public override JToken VisitMessageRaw(MessageRaw m)
        {
            return new JObject()
                {
                    new JProperty(nameof(m.Header), m.Header.Accept(this)),
                    new JProperty(nameof(m.Body), m.Body.Accept(this)),
                };
        }


        public override JToken VisitMessageHeader(MessageHeader m)
        {
            var array = new JArray();
            foreach (Message item in m.Items)
                array.Add(item.Accept(this));
            return array;
        }


        public override JToken VisitMessageBlock(MessageBlock m)
        {

            if (m.IsArray)
            {
                var array = new JArray();
                foreach (Message item in m.Items)
                    array.Add(item.Accept(this));
                return array;

            }
            else
            {
                var obj = new JObject();
                foreach (Message item in m.Items)
                    obj.Add(item.Accept(this));
                return obj;
            }

        }


        public override JToken VisitMessageProperty(MessageProperty m)
        {
            return new JObject()
                {
                    new JProperty(nameof(m.Name), new JValue(m.Name)),
                    new JProperty(nameof(m.Value), m.Accept(this)),
                };
        }


        public override JToken VisitMessageValue(MessageValue m)
        {
            return new JObject()
                {
                    new JProperty(nameof(m.Value), new JValue(m.Value)),
                    new JProperty(nameof(m.Type), new JValue(m.Type)),
                };
        }
    }

}



