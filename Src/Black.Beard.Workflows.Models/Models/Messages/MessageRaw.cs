﻿namespace Bb.Workflows.Models.Messages
{
    public class MessageRaw : Message
    {

        public MessageHeader Header { get; set; }
        
        public MessageBlock Body { get; set; }

        public override T Accept<T>(MessageVisitor<T> visitor)
        {
            return visitor.VisitMessageRaw(this);
        }

    }

}