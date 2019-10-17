using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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



        static ExpressionHelper()
        {

            HashSet<string> names = new HashSet<string>()
            {
                "ToBoolean",
                "ToByte",
                "ToChar",
                "ToDateTime",
                "ToDecimal",
                "ToDouble",
                "ToInt16",
                "ToInt32",
                "ToInt64",
                "ToSByte",
                "ToSingle",
                "ToString",
                "ToUInt16",
                "ToUInt32",
                "ToUInt64",
                "ChangeType",
            };

            var ms = typeof(Convert).GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            foreach (var item in ms)
            {
                if (!names.Contains(item.Name))
                    continue;

                var p = item.GetParameters();
                if ((p.Length == 1 || p.Length == 2) && p[0].ParameterType != item.ReturnType)
                {
                    if (!_dicConverters.TryGetValue(p[0].ParameterType, out Dictionary<Type, MethodInfo> dic2))
                        _dicConverters.Add(p[0].ParameterType, dic2 = new Dictionary<Type, MethodInfo>());

                    if (!dic2.ContainsKey(item.ReturnType))
                        dic2.Add(item.ReturnType, item);

                    else
                    {

                    }
                }
            }

        }


        public static UnaryExpression Throw(this Type type, params Expression[] args) 
        {

            if (!typeof(Exception).IsAssignableFrom(type))
                throw new InvalidCastException($"{type.Name} don't inherit from Exception");

            List<Type> _types = new List<Type>();
            foreach (var arg in args)
                _types.Add(arg.ResolveType());

            var ctor = type.GetConstructor(_types.ToArray());
            if (ctor == null)
                throw new MissingMethodException(string.Join(", ", _types.Select(c => c.Name)));

            var result = Expression.Throw(Expression.New(ctor, args), typeof(NullReferenceException));

            return result;

        }

        public static Type ResolveType(this Expression self)
        {

            return self.NodeType == ExpressionType.Lambda
                ? (self as LambdaExpression).ReturnType
                : self.Type;

        }

        /// <summary>
        /// return an expression of convertion if targetype are differents
        /// </summary>
        /// <param name="self"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Expression ConvertIfDifferent(this Expression self, Type targetType)
        {

            Expression result = null;
            Type sourceType = self.ResolveType();

            if (sourceType != targetType)
            {
                try
                {
                    result = Expression.Convert(self, targetType);
                }
                catch (Exception) // not managed
                {
                    if (_dicConverters.TryGetValue(sourceType, out Dictionary<Type, MethodInfo> dic2))
                        if (dic2.TryGetValue(targetType, out MethodInfo method))
                        {

                            var parameters = method.GetParameters();
                            if (parameters.Length == 1)
                                result = Expression.Call(null, method, self);
                            else
                            {

                                if (parameters[1].ParameterType == typeof(IFormatProvider))
                                    result = Expression.Call(null, method, self, Expression.Constant(CultureInfo.CurrentCulture));

                                else if (parameters[1].ParameterType == typeof(Type))
                                    result = Expression.Call(null, method, self, Expression.Constant(targetType));

                                else
                                    result = Expression.Call(null, method, self);

                            }
                        }

                    if (result == null)
                    {
                        if (targetType != typeof(object))
                        {
                            result = self.ConvertIfDifferent(typeof(object));
                            result = result.ConvertIfDifferent(targetType);
                        }
                        else throw;
                    }

                }
            }
            else
                result = self;

            return result;

        }


        public static Func<object, T> GetConstant<T>(object value)
        {
            Expression cc = Expression.Constant(value);
            if (cc.Type != typeof(T))
                cc = Expression.Convert(cc, typeof(T));
            ParameterExpression arg0 = Expression.Parameter(typeof(object), "arg0");
            var lbd = Expression.Lambda<Func<object, T>>(cc, arg0);
            return lbd.Compile();
        }

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

                    var method = lastInstanceType.GetMethod("GetWithPath", new Type[] { typeof(Queue<string>), typeof(object) });
                    var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, arg0));
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

                        blk.Add(Expression.Assign(p, Expression.Property(_last, "ExtendedDatas")));

                        if (!vars.TryGetValue(typeof(string), out parameterResult))
                            vars.Add(typeof(object), (parameterResult = Expression.Parameter(typeof(object), "_" + typeof(string).Name.ToLower())));

                        var method = typeof(DynObject).GetMethod("GetWithPath", new Type[] { typeof(Queue<string>), typeof(RunContext) });
                        var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, arg0));
                        blk.Add(j);

                    }
                    else
                    {

                        if (!vars.TryGetValue(property.PropertyType, out parameterResult))
                            vars.Add(property.PropertyType, (parameterResult = Expression.Parameter(property.PropertyType, "_" + property.PropertyType.Name.ToLower())));

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


        private static Dictionary<Type, Dictionary<Type, MethodInfo>> _dicConverters = new Dictionary<Type, Dictionary<Type, MethodInfo>>();

    }

}
