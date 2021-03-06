﻿using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models.Messages
{
    public class MessageBlock : Message
    {

        public List<Message> Items { get; set; } = new List<Message>();

        public bool IsArray { get;  set; }

        public override T Accept<T>(MessageVisitor<T> visitor)
        {
            return visitor.VisitMessageBlock(this);
        }

        internal MessageBlock Add(string name, object value)
        {
            return Add(new MessageProperty() { Name = name, Value = new MessageValue() { Value = value } });
        }

        internal MessageBlock Add(MessageProperty messageProperty)
        {
            this.Items.Add(messageProperty);
            return this;
        }

    }

}
