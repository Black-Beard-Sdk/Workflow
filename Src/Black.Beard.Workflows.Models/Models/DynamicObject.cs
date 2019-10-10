using System;
using System.Collections.Generic;

namespace Bb.Workflows.Models
{
    public class DynamicObject
    {

        public static DynamicObject None = new DynamicObject();

        public Dictionary<string, DynamicObject> Items { get; set; } = new Dictionary<string, DynamicObject>();


        public bool Check(string key, string value)
        {
            
            if (Items.TryGetValue(key, out DynamicObject o) && o.Value == value)
                return true;

            return false;

        }

        public bool ContainsKey(string key)
        {
            return Items.ContainsKey(key);
        }

        public DynamicObject this[string key]
        {
            get 
            {
                if (Items.TryGetValue(key, out DynamicObject o))
                    return o;
                return None;
            }
        }

        public string GetWithPath(Queue<string> properties)
        {

            if (properties.Count > 0)
            {
                string propertyName = properties.Dequeue();
                if (Items.TryGetValue(propertyName, out DynamicObject d))
                    return d.GetWithPath(properties);
                else
                    throw new Exceptions.ResolutionArgumentException(propertyName);
            }

            return Value;

        }

        public string Value { get; set; }

        public T ValueAs<T>()
        {
            return (T)Convert.ChangeType(this.Value, typeof(T));
        }

        public bool IsArray { get; set; }


        public DynamicObject Clone()
        {

            var r = new DynamicObject()
            {
                Value = Value,
                IsArray = IsArray,
            };

            foreach (var item in Items)
                r.Items.Add(item.Key, item.Value.Clone());

            return r;

        }

    }

}
