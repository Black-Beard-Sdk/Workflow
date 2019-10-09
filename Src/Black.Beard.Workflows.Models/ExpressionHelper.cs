using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Workflows
{

    /// <summary>
    /// build lanbda and compile for ActionResult 
    /// </summary>
    public static class ExpressionHelper
    {

        public static Func<object, T> GetConstant<T>(object value)
        {
            Expression cc = Expression.Constant(value);
            if (cc.Type != typeof(T))
                cc = Expression.Convert(cc, typeof(T));
            ParameterExpression arg0 = Expression.Parameter(typeof(object), "arg0");
            var lbd = Expression.Lambda<Func<object, T>>(cc, arg0);
            return lbd.Compile();
        }

        public static Func<object, object> GetAccessors(Type type, string fullPath)
        {

            var ctor = typeof(NullReferenceException).GetConstructor(new Type[] { typeof(string) });

            Dictionary<Type, ParameterExpression> var = new Dictionary<Type, ParameterExpression>();
            List<Expression> blk = new List<Expression>();
            ParameterExpression arg0 = Expression.Parameter(typeof(object), "arg0");
            ParameterExpression currentInstance;
            Type lastInstanceType = type;
            currentInstance = Expression.Parameter(type, "_" + type.Name.ToLower());
            var.Add(currentInstance.Type, currentInstance);

            blk.Add(Expression.Assign(currentInstance, Expression.Convert(arg0, type)));

            var path = new Queue<string>(fullPath.Split('.'));
            PropertyInfo property;
            ParameterExpression parameterResult = null;

            while (path.Count > 0)
            {
                var memberName = path.Dequeue();
                if (lastInstanceType == typeof(DynamicObject))
                {

                    var _p = GetPath(path, memberName);

                    if (!var.TryGetValue(typeof(DynamicObject), out ParameterExpression p))
                        var.Add(typeof(DynamicObject), (p = Expression.Parameter(typeof(DynamicObject), "_" + type.Name.ToLower())));

                    if (!var.TryGetValue(typeof(string), out parameterResult))
                        var.Add(typeof(string), (parameterResult = Expression.Parameter(typeof(string), "_" + typeof(string).Name.ToLower())));

                    var method = lastInstanceType.GetMethod("GetWithPath", new Type[] { typeof(Queue<string>) });
                    var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p));
                    blk.Add(j);
                }
                else
                {

                    if (!var.TryGetValue(lastInstanceType, out ParameterExpression p))
                        var.Add(lastInstanceType, (p = Expression.Parameter(lastInstanceType, "_" + lastInstanceType.Name.ToLower())));

                    property = lastInstanceType.GetProperty(memberName);
                    if (property == null)
                    {

                        var _last = p;
                        var _p = GetPath(path, memberName);

                        if (!var.TryGetValue(typeof(DynamicObject), out p))
                            var.Add(typeof(DynamicObject), (p = Expression.Parameter(typeof(DynamicObject), "_" + typeof(DynamicObject).Name.ToLower())));

                        blk.Add(Expression.Assign(p, Expression.Property(_last, "ExtendedDatas")));

                        if (!var.TryGetValue(typeof(string), out parameterResult))
                            var.Add(typeof(string), (parameterResult = Expression.Parameter(typeof(string), "_" + typeof(string).Name.ToLower())));

                        var method = typeof(DynamicObject).GetMethod("GetWithPath", new Type[] { typeof(Queue<string>) });
                        var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p));
                        blk.Add(j);

                    }
                    else
                    {

                        if (!var.TryGetValue(property.PropertyType, out parameterResult))
                            var.Add(property.PropertyType, (parameterResult = Expression.Parameter(property.PropertyType, "_" + property.PropertyType.Name.ToLower())));

                        var e = Expression.Assign(parameterResult, Expression.Property(p, property));
                        blk.Add(e);

                        var i = Expression.IfThen(Expression.Equal(parameterResult, Expression.Constant(null)),
                            Expression.Throw(Expression.New(ctor, Expression.Constant(memberName)), typeof(NullReferenceException))
                            );
                        blk.Add(i);

                        lastInstanceType = property.PropertyType;
                    }
                }

            }

            blk.Add(Expression.Convert(parameterResult, typeof(object)));

            var b = Expression.Block(typeof(object), var.Values, blk.ToArray());
            var lbd = Expression.Lambda<Func<object, object>>(b, arg0);

            return lbd.Compile();

        }

        private static Expression GetPath(Queue<string> path, string memberName)
        {
            List<string> _l = new List<string>(path.Count + 1) { memberName };
            while (path.Count > 0)
                _l.Add(path.Dequeue());
            var ctor = typeof(Queue<string>).GetConstructor(new Type[] { typeof(IEnumerable<string>) });
            var arg = Expression.NewArrayInit(typeof(string), _l.Select(c => Expression.Constant(c)).ToArray());
            var a = Expression.New(ctor, arg);

            return a;

        }

    }

}
