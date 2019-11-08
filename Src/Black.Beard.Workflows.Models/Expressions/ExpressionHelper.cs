using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Workflows.Expresssions
{

    /// <summary>
    /// build lanbda and compile for ActionResult 
    /// </summary>
    public static class ExpressionDynobjectExtension
    {

        public static Func<T, object> GetAccessors<T>(string fullPath)
        {

            Type type = typeof(T);

            var ctor = typeof(NullReferenceException).GetConstructor(new Type[] { typeof(string) });


            Dictionary<Type, ParameterExpression> vars = new Dictionary<Type, ParameterExpression>();
            List<Expression> blk = new List<Expression>();
            ParameterExpression arg0 = Expression.Parameter(type, "arg0");
            Type lastInstanceType = type;
            vars.Add(type, arg0);

            var path = new Queue<string>(fullPath.Split('.'));
            PropertyInfo property;
            ParameterExpression parameterResult = arg0;

            while (path.Count > 0)
            {
                var memberName = path.Dequeue();
                if (lastInstanceType == typeof(DynObject))
                {

                    var _p = GetPath(path, memberName);

                    if (!vars.TryGetValue(typeof(DynObject), out ParameterExpression p))
                        vars.Add(typeof(DynObject), (p = Expression.Parameter(typeof(DynObject), "_" + type.Name.ToLower())));

                    if (!vars.TryGetValue(typeof(object), out parameterResult))
                        vars.Add(typeof(string), (parameterResult = Expression.Parameter(typeof(object), "_" + typeof(string).Name.ToLower())));

                    var method = lastInstanceType.GetMethod("GetWithPath", new Type[] { typeof(Queue<string>), typeof(object), typeof(string) });
                    var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, arg0, Expression.Constant(fullPath)));
                    blk.Add(j);
                }
                else
                {

                    if (!vars.TryGetValue(lastInstanceType, out ParameterExpression p))
                        vars.Add(lastInstanceType, (p = Expression.Parameter(lastInstanceType, "_" + lastInstanceType.Name.ToLower())));

                    property = lastInstanceType.GetProperty(memberName);
                    if (property == null)
                    {

                        var _last = p;
                        var _p = GetPath(path, memberName);

                        if (!vars.TryGetValue(typeof(DynObject), out p))
                            vars.Add(typeof(DynObject), (p = Expression.Parameter(typeof(DynObject), "_" + typeof(DynObject).Name.ToLower())));

                        var methodExtendedDatas = _last.Type.GetMethod("ExtendedDatas");

                        blk.Add(Expression.Assign(p, Expression.Call(_last, methodExtendedDatas)));

                        if (!vars.TryGetValue(typeof(string), out parameterResult))
                            vars.Add(typeof(object), (parameterResult = Expression.Parameter(typeof(object), "_" + typeof(string).Name.ToLower())));

                        var method = typeof(DynObject).GetMethod("GetWithPath", new Type[] { typeof(Queue<string>), typeof(RunContext), typeof(string) });
                        var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, arg0, Expression.Constant(fullPath)));
                        blk.Add(j);

                    }
                    else
                    {

                        if (!vars.TryGetValue(property.PropertyType, out parameterResult))
                            vars.Add(property.PropertyType, (parameterResult = Expression.Parameter(property.PropertyType, "_" + property.PropertyType.Name.ToLower())));

                        var e = Expression.Assign(parameterResult, Expression.Property(p, property));
                        blk.Add(e);

                        string msgError = $"key {memberName} is missing in {fullPath}";
                        var i = Expression.IfThen(Expression.Equal(parameterResult, Expression.Constant(null)),
                            Expression.Throw(Expression.New(ctor, Expression.Constant(msgError)), typeof(NullReferenceException))
                            );
                        blk.Add(i);

                        lastInstanceType = property.PropertyType;
                    }
                }

            }

            blk.Add(Expression.Convert(parameterResult, typeof(object)));

            var b = Expression.Block(typeof(object), vars.Values.Skip(1), blk.ToArray());
            var lbd = Expression.Lambda<Func<T, object>>(b, arg0);

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
