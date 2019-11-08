using System.Collections.Generic;
using System.Linq;

namespace Bb.Workflows.Models.Messages
{
    public class MessageHeader : Message
    {

        public MessageHeader()
        {

        }

        public MessageHeader(List<KeyValuePair<string, string>> headers)
        {
            if (headers != null)
                this.Items.AddRange(headers
                    .Select(c =>
                        new MessageProperty()
                        {
                            Name = c.Key,
                            Value = new MessageValue()
                            {
                                Value = c.Value,
                                Type = c.Value != null
                                    ? c.Value.GetType().Name
                                    : string.Empty
                            }
                        })
                    );
        }

        public List<MessageProperty> Items { get; set; } = new List<MessageProperty>();

        public override T Accept<T>(MessageVisitor<T> visitor)
        {
            return visitor.VisitMessageHeader(this);
        }

    }

}
