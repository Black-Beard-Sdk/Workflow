using System.Diagnostics;

namespace Bb.Workflows.Models.Messages
{
    [DebuggerDisplay("{Value}")]
    public class MessageValue : Message
    {

        public object Value { get; set; }

        public string Type { get; set; }

        public override T Accept<T>(MessageVisitor<T> visitor)
        {
            return visitor.VisitMessageValue(this);
        }

    }

}
