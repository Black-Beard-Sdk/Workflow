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



        public static CatchStatement Catch(this Type self)
        {

            return new CatchStatement()
            {
                TypeToCatch = self,
            };

        }

        public static CatchStatement Catch(this ParameterExpression self)
        {

            var c = new CatchStatement()
            {
                TypeToCatch = self.Type,
                Parameter = self,
            };

            c.Body.AddVar(self);

            return c;

        }



        public static NewArrayExpression NewArray(this Type self, IEnumerable<Expression> expressions)
        {
            return Expression.NewArrayInit(self, expressions);
        }

        public static NewArrayExpression NewArray(this Type self, params Expression[] expressions)
        {
            return Expression.NewArrayInit(self, expressions);
        }

        public static ConstantExpression AsConstant(this object self, Type type = null)
        {

            if (self is Expression e)
            {
                if (e is ConstantExpression c)
                    return c;
                else
                    throw new InvalidCastException("an expression can't be converted in constant");
            }

            if (type == null)
                return Expression.Constant(self);

            if (self != null && self.GetType() != type && self is IConvertible)
                self = Convert.ChangeType(self, type);

            return Expression.Constant(self, type);

        }

        //public static BinaryExpression Return(this Expression left)
        //{
        //    return Expression.Return(left);
        //}


        public static BinaryExpression AssignFrom(this Expression left, Expression right)
        {
            return Expression.Assign(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static NewExpression Create(this Type type, params Expression[] args)
        {

            List<Type> _types = new List<Type>();
            foreach (var arg in args)
                _types.Add(arg.ResolveType());

            var ctor = type.GetConstructor(_types.ToArray());
            if (ctor == null)
                throw new MissingMethodException(string.Join(", ", _types.Select(c => c.Name)));

            var result = Expression.New(ctor, args);

            return result;

        }

        public static MethodCallExpression Call(this MethodInfo self, params Expression[] arguments)
        {

            var parameters = self.GetParameters()
              .ToArray();

            List<Expression> _args = new List<Expression>(arguments.Length);

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                var parameter = parameters[i];

                _args.Add(argument.ConvertIfDifferent(parameter.ParameterType));

            }

            return Expression.Call(self, _args.ToArray());

        }

        public static MethodCallExpression Call(this Expression self, MethodInfo methodTarget, params Expression[] arguments)
        {

            var parameters = methodTarget.GetParameters()
              .ToArray();

            List<Expression> _args = new List<Expression>(arguments.Length);

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                var parameter = parameters[i];

                _args.Add(argument.ConvertIfDifferent(parameter.ParameterType));

                _args.Add(argument);
            }

            return Expression.Call(self, methodTarget, _args.ToArray());

        }

        public static UnaryExpression Throw(this Type type, params Expression[] args)
        {

            if (!typeof(Exception).IsAssignableFrom(type))
                throw new InvalidCastException($"{type.Name} don't inherit from Exception");

            var result = Expression.Throw(Create(type, args), typeof(NullReferenceException));

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

                if (self is ConstantExpression c)
                {
                    result = c.Value.AsConstant(targetType);
                    if (result.Type != targetType)
                        result = Expression.Convert(result, targetType);
                }
                else
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

        public static TypeBinaryExpression TypeEqual(this Expression left, Type type)
        {
            return Expression.TypeEqual(left, type);
        }

        public static TypeBinaryExpression TypeIs(this Expression left, Type type)
        {
            return Expression.TypeIs(left, type);
        }

        public static LoopExpression TypeIs(this Expression body)
        {
            return Expression.Loop(body);
        }


        public static DefaultExpression DefaultValue(this Type self)
        {
            return Expression.Default(self);
        }

        #region Binary expressions

        public static BinaryExpression Add(this Expression left, Expression right)
        {
            return Expression.Add(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression AddAssign(this Expression left, Expression right)
        {
            return Expression.AddAssign(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression And(this Expression left, Expression right)
        {
            return Expression.And(left, right);
        }

        public static BinaryExpression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }

        public static BinaryExpression AndAssign(this Expression left, Expression right)
        {
            return Expression.AndAssign(left, right);
        }

        public static BinaryExpression Coalesce(this Expression left, Expression right)
        {
            return Expression.Coalesce(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression Divide(this Expression left, Expression right)
        {
            return Expression.Divide(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression DivideAssign(this Expression left, Expression right)
        {
            return Expression.DivideAssign(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression Equal(this Expression left, Expression right)
        {
            return Expression.Equal(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression ExclusiveOr(this Expression left, Expression right)
        {
            return Expression.ExclusiveOr(left, right);
        }

        public static BinaryExpression ExclusiveOrAssign(this Expression left, Expression right)
        {
            return Expression.ExclusiveOrAssign(left, right);
        }

        public static BinaryExpression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression GreaterThanOrEqual(this Expression left, Expression right)
        {
            return Expression.GreaterThanOrEqual(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression LeftShift(this Expression left, Expression right)
        {
            return Expression.LeftShift(left, right);
        }

        public static BinaryExpression LeftShiftAssign(this Expression left, Expression right)
        {
            return Expression.LeftShiftAssign(left, right);
        }

        public static BinaryExpression LessThan(this Expression left, Expression right)
        {
            return Expression.LessThan(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression LessThanOrEqual(this Expression left, Expression right)
        {
            return Expression.LessThanOrEqual(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression Modulo(this Expression left, Expression right)
        {
            return Expression.Modulo(left, right);
        }

        public static BinaryExpression ModuloAssign(this Expression left, Expression right)
        {
            return Expression.ModuloAssign(left, right);
        }

        public static BinaryExpression Multiply(this Expression left, Expression right)
        {
            return Expression.Multiply(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression MultiplyAssign(this Expression left, Expression right)
        {
            return Expression.MultiplyAssign(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression NotEqual(this Expression left, Expression right)
        {
            return Expression.NotEqual(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression Or(this Expression left, Expression right)
        {
            return Expression.Or(left, right);
        }

        public static BinaryExpression OrAssign(this Expression left, Expression right)
        {
            return Expression.OrAssign(left, right);
        }

        public static BinaryExpression OrElse(this Expression left, Expression right)
        {
            return Expression.OrElse(left, right);
        }

        public static BinaryExpression Power(this Expression left, Expression right)
        {
            return Expression.Power(left, right);
        }

        public static BinaryExpression PowerAssign(this Expression left, Expression right)
        {
            return Expression.PowerAssign(left, right);
        }

        public static BinaryExpression RightShift(this Expression left, Expression right)
        {
            return Expression.RightShift(left, right);
        }

        public static BinaryExpression RightShiftAssign(this Expression left, Expression right)
        {
            return Expression.RightShiftAssign(left, right);
        }

        public static BinaryExpression Subtract(this Expression left, Expression right)
        {
            return Expression.Subtract(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        public static BinaryExpression SubtractAssign(this Expression left, Expression right)
        {
            return Expression.SubtractAssign(left, right.ConvertIfDifferent(left.ResolveType()));
        }

        #endregion Binary expressions

        #region Unary expression

        public static UnaryExpression TypeAs(this Expression left, Type type)
        {
            return Expression.TypeAs(left, type);
        }

        public static UnaryExpression Decrement(this Expression left)
        {
            return Expression.Decrement(left);
        }
        
        public static UnaryExpression Increment(this Expression left)
        {
            return Expression.Increment(left);
        }

        public static UnaryExpression IsTrue(this Expression left)
        {
            return Expression.IsTrue(left);
        }

        public static UnaryExpression IsFalse(this Expression left)
        {
            return Expression.IsFalse(left);
        }

        public static UnaryExpression Not(this Expression left)
        {
            return Expression.Not(left);
        }

        public static UnaryExpression Negate(this Expression left)
        {
            return Expression.Negate(left);
        }

        public static UnaryExpression PostDecrementAssign(this Expression left)
        {
            return Expression.PostDecrementAssign(left);
        }

        public static UnaryExpression PostIncrementAssign(this Expression left)
        {
            return Expression.PostIncrementAssign(left);
        }

        public static UnaryExpression PreDecrementAssign(this Expression left)
        {
            return Expression.PreDecrementAssign(left);
        }

        public static UnaryExpression PreIncrementAssign(this Expression left)
        {
            return Expression.PreIncrementAssign(left);
        }

        #endregion Unary expression


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
