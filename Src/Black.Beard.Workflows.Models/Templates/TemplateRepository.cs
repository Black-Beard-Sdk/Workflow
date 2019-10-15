using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Bb.Workflows.Templates
{

    public class TemplateRepository
    {

        public TemplateRepository(params Type[] types)
        {
            if (types != null && types.Length > 0)
                foreach (var type in types)
                    Add(type);
        }

        public void Add(Type type)
        {

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
                if (field.FieldType == typeof(Func<ResultAction, DynObject>))
                    AddTemplate(field.Name, (Func<ResultAction, DynObject>)field.GetValue(null));

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Static))
                if (property.PropertyType == typeof(Func<ResultAction, DynObject>))
                    AddTemplate(property.Name, (Func<ResultAction, DynObject>)property.GetValue(null));

        }


        public void AddTemplate(string key, DynObject o)
        {
            if (_dic.ContainsKey(key))
                _dic[key] = (r) => o.Clone();
            else
                _dic.Add(key, (r) => o.Clone());
        }


        public void AddTemplate(string key, Func<ResultAction, DynObject> o)
        {

            if (_dic.ContainsKey(key))
                _dic[key] = (r) => o(r)
                    .Clone()
                    .AddDefaultValues();
            else
                _dic.Add(key, (r) => o(r)
                    .Clone()
                    .AddDefaultValues()
                );
        }


        public DynObject Get(string key, ResultAction action = null)
        {

            if (this._dic.TryGetValue(key, out Func<ResultAction, DynObject> o))
                return o(action);

            if (DefaultAction != null)
                return DefaultAction(action);

            return DynObject.None;

        }

        public Func<ResultAction, DynObject> DefaultAction { get; set; }



        private Dictionary<string, Func<ResultAction, DynObject>> _dic = new Dictionary<string, Func<ResultAction, DynObject>>();

    }

}
