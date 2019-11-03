using System.Diagnostics;

namespace Bb.Workflows.Models.Messages
{
    [DebuggerDisplay("'{Name}' : {Value}")]
    public class MessageProperty : Message
    {

        public string Name { get; set; }

        public Message Value { get; set; }

        public override T Accept<T>(MessageVisitor<T> visitor)
        {
            return visitor.VisitMessageProperty(this);
        }

    }

}
