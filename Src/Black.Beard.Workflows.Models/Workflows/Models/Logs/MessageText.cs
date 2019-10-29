using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.Workflows.Models.Logs
{

    public class MessageText
    {

        public MessageText()
        {
        }

        public static MessageText Text(string key, int value)
        {
            return new MessageText().Add(key, value.ToString());
        }

        public static MessageText Text(string key, string value)
        {
            return new MessageText().Add(key, value);
        }

        public static MessageText Text(string key, Guid value)
        {
            return new MessageText().Add(key, value.ToString());
        }

        public MessageText Add(string key, Guid value)
        {
            return this.Add(key, value.ToString());
        }

        public MessageText Add(string key, DateTimeOffset value)
        {
            return this.Add(key, value.ToString());
        }

        public MessageText Add(string key, int value)
        {
            return this.Add(key, value.ToString());
        }

        public MessageText Add(string key, string value)
        {
            this._items.Add(new KeyValuePair<string, object>(key, value));
            return this;
        }

        public MessageText Add(string key, MessageText value)
        {
            this._items.Add(new KeyValuePair<string, object>(key, value));
            return this;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(3000);
            ToString_Impl(sb);
            return sb.ToString();
        }

        public void ToString_Impl(StringBuilder sb)
        {

            var _comma = string.Empty;

            sb.Append("{ ");

            foreach (var item in this._items)
            {
                sb.Append(_comma);
                sb.Append(@"""");
                sb.Append(item.Key);
                sb.Append(@""": ");

                if (item.Value is MessageText m)
                    m.ToString_Impl(sb);

                else
                {
                    sb.Append(@"""");
                    sb.Append(item.Value);
                    sb.Append(@"""");
                }

                _comma = ", ";

            }

            sb.Append(" }");
        }

        public static implicit operator string(MessageText txt)
        {
            return txt.ToString();
        }

        public static implicit operator MessageText((string, string) txt)
        {
            return MessageText.Text(txt.Item1, txt.Item2);
        }


        private List<KeyValuePair<string, object>> _items = new List<KeyValuePair<string, object>>();

    }

}
