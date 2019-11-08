using System;
using System.Collections.Generic;
using System.Text;
using Bb.ComponentModel;
using Bb.ComponentModel.Accessors;
using Bb.Workflows.Converters;
using Bb.Workflows.Models.Messages;

namespace Bb.Workflows.Models
{
    public class DynObject
    {

        #region fluent syntax

        public static DynObject Create(string value)
        {
            return new DynObject() { GetValue = (ctx) => value };
        }

        public static DynObject Create(Guid value)
        {
            return new DynObject() { GetValue = (ctx) => value };
        }

        public static DynObject Create(int value)
        {
            return new DynObject() { GetValue = (ctx) => value };
        }

        public static DynObject Create(decimal value)
        {
            return new DynObject() { GetValue = (ctx) => value };
        }

        public static DynObject Create(Func<RunContext, object> value)
        {
            return new DynObject()
            {
                GetValue = value,
            };
        }

        public static DynObject Create(string key, string value)
        {

            return new DynObject()
                .Add(key, value);
        }

        public static DynObject Create(string key, DynObject value)
        {

            return new DynObject()
                .Add(key, value);
        }

        public static DynObject Create(string key, Func<RunContext, object> value)
        {
            return new DynObject()
                .Add(key, value);
        }

        public DynObject Add(string key, string value)
        {
            if (this.Items.ContainsKey(key))
            {
                if (value == null)
                    this.Items.Add(key, new DynObject());
                else
                    this.Items.Add(key, new DynObject().SetValue(value));
            }
            else
            {
                if (value == null)
                    this.Items[key] = new DynObject();
                else
                    this.Items[key] = new DynObject().SetValue(value);
            }
            return this;
        }

        public DynObject Add(string key, Func<RunContext, object> value)
        {
            if (this.Items.ContainsKey(key))
                this.Items[key] = new DynObject() { GetValue = value };
            else
                this.Items.Add(key, new DynObject() { GetValue = value });
            return this;
        }

        public DynObject Apply(Action<DynObject> action)
        {

            action(this);

            foreach (var item in this.Items)
                item.Value.Apply(action);

            return this;

        }

        internal DynObject AddDefaultValues()
        {
            Add("EventDate", (ctx) => ctx.IncomingEvent.EventDate);
            Add("CreationDate", (ctx) => WorkflowClock.Now());
            Add("ExternalId", (ctx) => ctx.Workflow.ExternalId);
            return this;
        }

        public DynObject Add(string key, DynObject value)
        {
            this.Items.Add(key, value);
            return this;
        }

        public DynObject SetValue(string value)
        {
            if (value != null)
                this.GetValue = (ctx) => value;
            else
                this.GetValue = null;
            return this;
        }

        public DynObject AddSub(string key, IDictionary<string, DynObject> arguments)
        {
            var arg = new DynObject();
            this.Items.Add(key, arg);
            if (arguments != null)
            {
                foreach (var item in arguments)
                    arg.Add(item.Key, item.Value);
            }
            return arg;
        }

        public DynObject Add(IDictionary<string, DynObject> arguments)
        {
            foreach (var item in arguments)
                Items.Add(item.Key, item.Value);
            return this;
        }

        public Message Resolve(RunContext ctx)
        {

            Message r = null;
            if (this.GetValue != null)
            {
                var r1 = new MessageValue()
                {
                    Value = this.GetValue(ctx),
                };

                r1.Type = r1.Value != null
                    ? r1.Value.GetType().Name
                    : string.Empty;

                r = r1;

            }
            else
            {

                var r2 = new MessageBlock()
                {
                    IsArray = IsArray,
                };

                foreach (var item in Items)
                {

                    var value = item.Value.Resolve(ctx);

                    if (IsArray)
                        r2.Items.Add(value);

                    else
                        r2.Items.Add(new MessageProperty()
                        {
                            Name = item.Key,
                            Value = value,
                        });

                }

                r = r2;

            }

            return r;

        }

        //public void MergeToObject(IExtendedDatas instance, RunContext ctx = null)
        //{

        //    var properties = TypeDiscovery.Instance.GetProperties(instance.GetType());

        //    foreach (var item in this.Items)
        //        if (properties.TryGetValue(item.Key, out AccessorItem acc))
        //        {
        //            //acc.Type
        //            var txt = item.Value.Serialize(ctx);
        //        }
        //        else
        //            instance.ExtendedDatas.Add(item.Key, item.Value.Clone());

        //}

        public DynObject Merge(DynObject o)
        {

            if (o.GetValue != null)
            {
                if (this.GetValue != null)
                    throw new Exceptions.ConflictMergeDynObjectException("object can't be merged because the value is not empty.");
                else
                    GetValue = o.GetValue;
            }
            else
                foreach (var item in o.Items)
                    if (Items.TryGetValue(item.Key, out DynObject o2))
                        o2.Merge(item.Value);
                    else
                        Items.Add(item.Key, item.Value);

            return this;
        }


        #endregion fluent syntax

        public bool Check(string key, string value, RunContext ctx)
        {

            if (Items.TryGetValue(key, out DynObject o) && o.GetValue(ctx).ToString() == value)
                return true;

            return false;

        }

        public bool ContainsKey(string key)
        {
            return Items.ContainsKey(key);
        }

        public DynObject this[string key]
        {
            get
            {
                if (Items.TryGetValue(key, out DynObject o))
                    return o;
                return None;
            }
        }

        public object GetWithPath(Queue<string> properties, RunContext ctx, string fullpath)
        {

            if (properties.Count > 0)
            {
                string propertyName = properties.Dequeue();
                if (Items.TryGetValue(propertyName, out DynObject d))
                    return d.GetWithPath(properties, ctx, fullpath);
                else
                    throw new Exceptions.ResolutionArgumentException($"missing key {propertyName} in path {fullpath}");
            }

            return GetValue(ctx);

        }


        public Dictionary<string, DynObject> Items { get; set; } = new Dictionary<string, DynObject>();

        //public string Value { get; set; }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Diagnostics.DebuggerNonUserCode]
        public T ValueAs<T>(RunContext ctx)
        {
            if (this.GetValue == null)
                throw new Exceptions.MissingArgumentNameMethodReferenceException("value is not initialized");
            return (T)Convert.ChangeType(this.GetValue(ctx), typeof(T));
        }

        public bool IsArray { get; set; }

        public Func<RunContext, object> GetValue { get; private set; }

        public DynObject Clone()
        {

            var r = new DynObject()
            {
                GetValue = this.GetValue,
                IsArray = IsArray,
            };

            foreach (var item in Items)
                r.Items.Add(item.Key, item.Value.Clone());

            return r;

        }

        public StringBuilder Serialize(RunContext ctx)
        {

            StringBuilder sb = new StringBuilder(1000);

            if (IsArray)
                sb.Append("[");
            else
                sb.Append("{");

            Serialize(ctx, sb);

            if (IsArray)
                sb.Append("]");
            else
                sb.Append("}");

            return sb;
        }

        private void Serialize(RunContext ctx, StringBuilder sb)
        {

            string comma = string.Empty;

            if (GetValue != null)
            {

                var value = GetValue(ctx);

                if (value is string)
                {
                    sb.Append(@"""");
                    sb.Append(value);
                    sb.Append(@"""");
                }
                else
                    sb.Append(value);

            }
            else
            {

                if (IsArray)
                    sb.Append("[");
                else
                    sb.Append("{");


                foreach (var item in Items)
                {

                    sb.Append(comma);

                    if (!IsArray)
                    {
                        sb.Append(@"""");
                        sb.Append(item.Key);
                        sb.Append(@""" : ");
                    }

                    item.Value.Serialize(ctx, sb);

                    comma = ", ";
                }

                if (IsArray)
                    sb.Append("]");
                else
                    sb.Append("}");

            }

        }

        public static DynObject None = new DynObject();

    }

}
