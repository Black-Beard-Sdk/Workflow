using System;
using System.Text;

namespace Bb.Workflows.Models.Messages
{

    public abstract class Message
    {

        public abstract T Accept<T>(MessageVisitor<T> visitor);

    }

}
