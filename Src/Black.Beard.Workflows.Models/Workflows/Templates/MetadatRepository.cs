using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bb.Workflows.Templates
{


    public static class MetadataModels
    {


        public static List<KeyValuePair<string, string>> Reminder = new List<KeyValuePair<string, string>>()
        {

        };


        public static List<KeyValuePair<string, string>> __cancelReminderAction = new List<KeyValuePair<string, string>>()
        {

        };


        public static List<KeyValuePair<string, string>> __expired = new List<KeyValuePair<string, string>>()
        {

        };


        public static List<KeyValuePair<string, string>> DefaultAction = new List<KeyValuePair<string, string>>()
        {

        };


    }

    public class MetadatRepository
    {

        public MetadatRepository(params Type[] types)
        {
            if (types != null && types.Length > 0)
                foreach (var type in types)
                    Add(type);
        }

        public void Add(Type type)
        {

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                if (field.FieldType == typeof(IEnumerable<KeyValuePair<string, string>>))
                    AddMetadata(field.Name, (IEnumerable<KeyValuePair<string, string>>)field.GetValue(null));

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static))
                if (property.PropertyType == typeof(IEnumerable<KeyValuePair<string, string>>))
                    AddMetadata(property.Name, (IEnumerable<KeyValuePair<string, string>>)property.GetValue(null));

        }

        public void AddMetadata(string key, IEnumerable<KeyValuePair<string, string>> metadatas)
        {

            if (!_dic.TryGetValue(key, out Dictionary<string, string> list))
                _dic.Add(key, list = new Dictionary<string, string>());

            foreach (var metadata in metadatas)
                if (list.ContainsKey(metadata.Key))
                    list[key] = metadata.Value;
                else
                    list.Add(key, metadata.Value);

        }

        public List<KeyValuePair<string, string>> Get(string name)
        {

            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();

            if (_dic.TryGetValue(name, out Dictionary<string, string> list))
            {
                foreach (var item in list)
                    result.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }
            else
            {
                foreach (var item in DefaultAction)
                    result.Add(new KeyValuePair<string, string>(item.Key, item.Value));

            }
            return result;

        }

        public Dictionary<string, string> DefaultAction { get; set; }

        private Dictionary<string, Dictionary<string, string>> _dic = new Dictionary<string, Dictionary<string, string>>();


    }

}
