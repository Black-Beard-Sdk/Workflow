using Newtonsoft.Json.Linq;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Messages;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System;
using Bb.ComponentModel.Accessors;
using Bb.ComponentModel;

namespace Bb.Workflows.Converters
{
    public static class DynObjectSerializer
    {

        public static string Serialize(IExtendedDatas instance, Type typeBase)
        {

            if (instance == null)
                throw new NullReferenceException(nameof(instance));

            var propertiesToExcludes = TypeDiscovery.Instance.GetProperties(typeBase);

            var result = JObject.FromObject(instance);

            foreach (var item in propertiesToExcludes)
                if (result.ContainsKey(item.Name))
                    result.Remove(item.Name);

            var u = Serialize(instance.ExtendedDatas());
            result.Merge(u);

            return result.ToString(Newtonsoft.Json.Formatting.None);

        }


        public static void Deserialize(IExtendedDatas instance, string text)
        {

            if (instance == null)
                throw new NullReferenceException(nameof(instance));

            Type type = instance.GetType();

            var list = TypeDiscovery.Instance.GetProperties(type);

            JObject o = JObject.Parse(text);

            foreach (var item in o)
                if (!list.ContainsKey(item.Key))
                {
                    if (item.Value is JValue v)
                        instance.ExtendedDatas().Add(item.Key, v.Value.ToString());

                    else if (item.Value is JObject ob)
                    {
                        DynObject d = new DynObject();
                        instance.ExtendedDatas().Add(item.Key, d);
                        Deserialize(ob, d);
                    }

                    else if (item.Value is JArray a)
                    {
                        DynObject d = new DynObject();
                        instance.ExtendedDatas().Add(item.Key, d);
                        Deserialize(a, d);
                    }
                }
        }

        public static void Deserialize(string text, DynObject obj)
        {
            var o = JObject.Parse(text);
            Deserialize(o, obj);
        }

        public static DynObject Deserialize(string text)
        {
            var o = JObject.Parse(text);
            var result = new DynObject();
            Deserialize(o, result);
            return result;
        }

        public static void Deserialize(JArray v, DynObject o)
        {

            o.IsArray = true;

            foreach (var item in v)
            {

                var j = new DynObject() { };
                o.Items.Add(o.Items.Count.ToString(), j);

                if (item is JValue v1)
                    j.SetValue(v1.Value.ToString());

                else if (item is JObject v2)
                    Deserialize(v2, j);

                else if (item is JArray v3)
                    Deserialize(v3, j);

            }
        }

        public static void Deserialize(JObject v, DynObject o)
        {

            foreach (var item in v)
            {

                var j = new DynObject() { };
                o.Items.Add(item.Key, j);

                if (item.Value is JValue v1)
                    j.SetValue(v1.Value.ToString());

                else if (item.Value is JObject v2)
                    Deserialize(v2, o);

                else if (item.Value is JArray v3)
                    Deserialize(v3, o);

                else
                {

                }

            }

        }



        public static string SerializeToText(this DynObject value)
        {

            var result = value.Serialize();
            if (result == null)
                return string.Empty;

            return result.ToString(Newtonsoft.Json.Formatting.None);

        }

        public static JToken Serialize(this DynObject value)
        {

            JToken result = null;
            object v = value.GetValue != null ? value.GetValue(null) : null;
            if (v != null)
            {
                if (v is string s)
                {
                    if (!string.IsNullOrEmpty(s))
                        result = new JValue(v);
                }
                else
                    result = new JValue(v);
            }
            else
            {

                if (value.IsArray)
                {
                    var ar = new JArray();
                    foreach (var item in value.Items)
                    {
                        var r = Serialize(item.Value);
                        if (r != null)
                            ar.Add(r);
                    }
                    if (ar.Count > 0)
                        result = ar;
                }
                else
                {
                    var oj = new JObject();
                    foreach (var item in value.Items)
                    {
                        var r = Serialize(item.Value);
                        if (r != null)
                            oj.Add(new JProperty(item.Key, r));
                    }
                    if (oj.Count > 0)
                        result = oj;
                }

            }

            if (result == null)
                result = new JObject();

            return result;

        }

        public static JToken Serialize(this MessageRaw self)
        {
            return visitor.VisitMessageRaw(self);
        }

        private static VisitorJsonSerialiser visitor = new VisitorJsonSerialiser();

    }

}



