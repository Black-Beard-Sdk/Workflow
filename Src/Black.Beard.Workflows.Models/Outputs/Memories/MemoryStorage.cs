using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Workflows.Outputs
{

    public class MemoryStorage
    {

        public MemoryStorage()
        {
            items = new Dictionary<Type, Box>();
        }

        public T Get<T>(Guid key)
        {

            if (items.TryGetValue(typeof(T), out Box b))
                return (T)b.Get(key);

            return default(T);

        }

        public IEnumerable<T> GetBy<T, TU>(string key, Expression<Func<T, TU>> e)
        {
            if (items.TryGetValue(typeof(T), out Box b))
            {
                var member = MemberResolver.GetMember(e) as PropertyInfo;
                foreach (var item in b.GetBy<T>(key, member))
                    yield return item;
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            if (items.TryGetValue(typeof(T), out Box b))
                foreach (T item in b.GetAll<T>())
                    yield return item;
        }

        public void Save<T>(Guid key, T item)
        {

            if (!items.TryGetValue(typeof(T), out Box b))
                lock (_lock)
                    if (!items.TryGetValue(typeof(T), out b))
                        items.Add(typeof(T), b = new Box());

            b.Save(key, item);

        }

        public void Remove<T>(Guid key)
        {

            if (items.TryGetValue(typeof(T), out Box b))
                b.Remove(key);

        }


        private volatile object _lock = new object();
        private Dictionary<Type, Box> items;

        private class Box
        {

            public Box()
            {
                Items = new Dictionary<Guid, object>();
            }

            public Dictionary<Guid, object> Items { get; private set; }

            internal void Save(Guid key, object item)
            {
                lock (_lock)
                    if (Items.ContainsKey(key))
                        Items[key] = item;
                    else
                        Items.Add(key, item);
            }

            internal void Remove(Guid key)
            {
                if (Items.ContainsKey(key))
                    lock (_lock)
                        Items.Remove(key);
            }

            internal object Get(Guid key)
            {
                Items.TryGetValue(key, out object result);
                return result;
            }

            internal IEnumerable<T> GetBy<T>(string key, PropertyInfo member)
            {
                foreach (var item in Items.Values)
                    if (Equals(member.GetValue(item), key))
                        yield return (T)item;
            }

            internal IEnumerable<T> GetAll<T>()
            {
                foreach (var item in Items.Values)
                    yield return (T)item;
            }


            private volatile object _lock = new object();

        }

        private class MemberResolver : ExpressionVisitor
        {

            private MemberResolver()
            {

            }

            public static MemberInfo GetMember(Expression e)
            {
                MemberResolver visitor = new MemberResolver();
                visitor.Visit(e);
                return visitor.member;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                this.member = node.Member;
                return base.VisitMember(node);
            }

            private MemberInfo member;

        }

    }

}
