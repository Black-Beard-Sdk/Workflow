using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Workflows.Parser.Models
{

    /// <summary>
    /// Contain a method discovered
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    [System.Diagnostics.DebuggerDisplay("{RuleName}")]
    public class BusinessAction<TContext>
    {
        private static readonly MethodInfo _methodLogResult;

        static BusinessAction()
        {
            BusinessAction<TContext>._methodLogResult = typeof(BusinessAction<TContext>).GetMethod("LogResult", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public BusinessAction()
        {

        }

        /// <summary>
        /// Return an expression from the method
        /// </summary>
        /// <param name="argumentContext"></param>
        /// <returns></returns>
        public Expression GetCallAction(params Expression[] arguments)
        {

            // build custom method
            List<Expression> _args = new List<Expression>(arguments.Length);
            var parameters = this.Method.GetParameters()
                .ToArray();

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                var parameter = parameters[i];

                if (argument.Type != parameter.ParameterType)
                {

                    if (argument is ConstantExpression c)
                        argument = Expression.Constant(Convert.ChangeType(c.Value, parameter.ParameterType));

                    if (argument is ParameterExpression p)
                        argument = Expression.Convert(p, parameter.ParameterType);

                    else
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                            System.Diagnostics.Debugger.Break();
                        argument = Expression.Convert(argument, parameter.ParameterType);
                    }

                }

                _args.Add(argument);

            }

            var m = Expression.Call(this.Method, _args.ToArray());


            // Build log method
            List<Expression> _args2 = new List<Expression>(arguments.Length + 2);
            _args2.Add(Expression.Constant(this.RuleName));
            _args2.Add(m);

            List<string> _args3 = new List<string>(4);

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];

                if (argument.Type == typeof(TContext))
                    _args2.Add(argument);

                else if (argument is ConstantExpression c)
                    _args3.Add(c.Value.ToString());

                else
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                        System.Diagnostics.Debugger.Break();
                }

            }

            _args2.Add(Expression.Constant(_args3.ToArray()));
            var m2 = Expression.Call(_methodLogResult, _args2.ToArray());

            return m2;

        }


        public static Expression<Func<TContext, object>> GetAccessorToArgument(string fullPath, ParameterExpression arg0)
        {

            Type type = typeof(TContext);

            var ctor = typeof(NullReferenceException).GetConstructor(new Type[] { typeof(string) });

            Dictionary<Type, ParameterExpression> varDic = new Dictionary<Type, ParameterExpression>();
            List<Expression> blk = new List<Expression>();
            ParameterExpression currentInstance;
            Type lastInstanceType = type;
            currentInstance = Expression.Parameter(type, "_" + type.Name.ToLower());
            varDic.Add(currentInstance.Type, currentInstance);


            blk.Add(Expression.Assign(currentInstance, Expression.Convert(arg0, type)));

            var path = new Queue<string>(fullPath.Split('.'));
            PropertyInfo property;
            ParameterExpression parameterResult = null;

            while (path.Count > 0)
            {
                var memberName = path.Dequeue();
                if (lastInstanceType == typeof(DynObject))
                {

                    var _p = GetPath(path, memberName);

                    if (!varDic.TryGetValue(typeof(DynObject), out ParameterExpression p))
                        varDic.Add(typeof(DynObject), (p = Expression.Parameter(typeof(DynObject), "_" + type.Name.ToLower())));

                    if (!varDic.TryGetValue(typeof(string), out parameterResult))
                        varDic.Add(typeof(string), (parameterResult = Expression.Parameter(typeof(string), "_" + typeof(string).Name.ToLower())));

                    var method = lastInstanceType.GetMethod("GetWithPath", new Type[] { typeof(Queue<string>) });
                    var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, arg0));
                    blk.Add(j);
                }
                else
                {

                    if (!varDic.TryGetValue(lastInstanceType, out ParameterExpression p))
                        varDic.Add(lastInstanceType, (p = Expression.Parameter(lastInstanceType, "_" + lastInstanceType.Name.ToLower())));

                    property = lastInstanceType.GetProperty(memberName);
                    if (property == null)
                    {

                        var _last = p;
                        var _p = GetPath(path, memberName);

                        if (!varDic.TryGetValue(typeof(DynObject), out p))
                            varDic.Add(typeof(DynObject), (p = Expression.Parameter(typeof(DynObject), "_" + typeof(DynObject).Name.ToLower())));

                        blk.Add(Expression.Assign(p, Expression.Property(_last, "ExtendedDatas")));

                        if (!varDic.TryGetValue(typeof(string), out parameterResult))
                            varDic.Add(typeof(string), (parameterResult = Expression.Parameter(typeof(string), "_" + typeof(string).Name.ToLower())));

                        var method = typeof(DynObject).GetMethod("GetWithPath", new Type[] { typeof(Queue<string>) });
                        var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, arg0));
                        blk.Add(j);

                    }
                    else
                    {

                        if (!varDic.TryGetValue(property.PropertyType, out parameterResult))
                            varDic.Add(property.PropertyType, (parameterResult = Expression.Parameter(property.PropertyType, "_" + property.PropertyType.Name.ToLower())));

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

            blk.Add(parameterResult);

            var b = Expression.Block(typeof(object), varDic.Values, blk.ToArray());
            var lbd = Expression.Lambda<Func<TContext, object>>(b, arg0);

            return lbd;

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

        /// <summary>
        /// Attention on y fait bien référence par reflexion dans la methode précédente.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="result"></param>
        /// <param name="context"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static bool LogResult(string ruleName, bool result, TContext context, string[] arguments)
        {

            if (FunctionalLog == null)
            {
                string message = $"{ruleName}({string.Join(", ", arguments)}) => {result}";
                Trace.WriteLine(message);
            }
            else
                FunctionalLog(ruleName, result, context, arguments);

            return result;
        }

        public static Func<string, bool, TContext, string[], bool> FunctionalLog;


        /// <summary>
        /// Return an expression from the method
        /// </summary>
        /// <param name="argumentContext"></param>
        /// <returns></returns>
        public Expression GetLoadDatasAction(params Expression[] arguments)
        {

            // build custom method
            List<Expression> _args = new List<Expression>(arguments.Length);
            var parameters = this.Method.GetParameters();

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                var parameter = parameters[i];

                if (argument.Type != parameter.ParameterType)
                {

                    if (argument is ConstantExpression c)
                        argument = Expression.Constant(Convert.ChangeType(c.Value, parameter.ParameterType));

                    else
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                            System.Diagnostics.Debugger.Break();
                        argument = Expression.Convert(argument, parameter.ParameterType);
                    }

                }
                _args.Add(argument);
            }

            var m = Expression.Call(this.Method, _args.ToArray());


            //// Build log method
            //List<Expression> _args2 = new List<Expression>(arguments.Length + 2);
            //_args2.Add(Expression.Constant(this.RuleName));
            //_args2.Add(m);

            //List<string> _args3 = new List<string>(4);

            //for (int i = 0; i < arguments.Length; i++)
            //{
            //    var argument = arguments[i];

            //    if (argument.Type == typeof(TContext))
            //        _args2.Add(argument);

            //    else if (argument is ConstantExpression c)
            //        _args3.Add(c.Value.ToString());

            //    else
            //    {
            //        if (System.Diagnostics.Debugger.IsAttached)
            //            System.Diagnostics.Debugger.Break();
            //    }

            //}

            //_args2.Add(Expression.Constant(_args3.ToArray()));

            //MethodInfo method2 = typeof(BusinessAction<TContext>).GetMethod("LogResult", BindingFlags.Static | BindingFlags.Public);

            //var m2 = Expression.Call(method2, _args2.ToArray());


            return m;

        }

        public MethodInfo Method { get; internal set; }

        public string RuleName { get; internal set; }

        //public Type Type { get; internal set; }

        //public string Origin { get; internal set; }

    }

}
