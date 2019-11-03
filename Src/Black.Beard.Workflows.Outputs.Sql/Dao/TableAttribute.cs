using System;

namespace Bb.Dao
{
    internal class TableAttribute : Attribute
    {
        public string Name { get; internal set; }
        public string Schema { get; internal set; }
    }
}