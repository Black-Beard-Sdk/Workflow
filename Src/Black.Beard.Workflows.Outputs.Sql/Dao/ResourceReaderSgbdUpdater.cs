using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Bb.Dao
{
    public class ResourceReaderSgbdUpdater : ISgbdUpdater
    {

        public ResourceReaderSgbdUpdater(Type type, CultureInfo culture) 
            : this(type.FullName, type.Assembly, culture)
        {

        }

        public ResourceReaderSgbdUpdater(string baseName, Type type, CultureInfo culture) 
            : this(baseName, type.Assembly, culture)
        {

        }

        public ResourceReaderSgbdUpdater(string baseName, Assembly assembly, CultureInfo culture)
        {

            _baseName = baseName;
            _assembly = assembly;
            _culture = culture ?? CultureInfo.CurrentCulture;

        }

        private List<string> ResolveKeys()
        {

            List<string> keys = new List<string>(100);
            System.IO.Stream fs = _assembly.GetManifestResourceStream(_baseName + ".resources");

            using (var streamResx = new System.Resources.ResourceSet(fs))
            {
                IDictionaryEnumerator enumerator = streamResx.GetEnumerator();
                while (enumerator.MoveNext())
                    keys.Add(enumerator.Key.ToString());
            }

            keys.Sort();

            return keys;

        }

        public StringBuilder[] GetScripts(Dictionary<string, string> arguments)
        {

            if (_keys == null)
                _keys = ResolveKeys();

            if (_manager == null)
                _manager = new global::System.Resources.ResourceManager(_baseName, _assembly);

            if (arguments == null)
                arguments = new Dictionary<string, string>();

            List<StringBuilder> _datas = new List<StringBuilder>();
            foreach (var item in _keys)
            {

                var txt = _manager.GetString(item, _culture);
                foreach (var argument in arguments)
                    txt = txt.Replace($"{{{{{argument.Key}}}}}", argument.Value);

                var data = new StringBuilder(txt);
                _datas.Add(data);

            }

            return _datas.ToArray();

        }

        private readonly CultureInfo _culture;
        private ResourceManager _manager;
        private readonly string _baseName;
        private readonly Assembly _assembly;
        private List<string> _keys;
    }

}