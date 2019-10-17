using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

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
            List<Expression> _argsLog = new List<Expression>(4);
            _argsLog.Add(Expression.Constant(this.RuleName));
            _argsLog.Add(m);
            List<Expression> _argValues = new List<Expression>();
            List<Expression> _argNames = new List<Expression>();

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                if (argument.Type == typeof(TContext))
                    _argsLog.Add(argument);
                else
                {
                    _argNames.Add(Expression.Constant(parameters[i].Name));
                    _argValues.Add(argument.ConvertIfDifferent(typeof(object)));
                }
            }

            _argsLog.Add(Expression.NewArrayInit(typeof(string), _argNames));
            _argsLog.Add(Expression.NewArrayInit(typeof(object), _argValues));

            var m2 = Expression.Call(_methodLogResult, _argsLog.ToArray());
            return m2;

        }

        public static Expression<Func<TContext, object>> GetAccessorToArgument2(string fullPath, ParameterExpression arg0)
        {
            var result = GetAccessorToArgumentGenerateCode(fullPath, arg0);
         
            result.Item2.Add(result.Item1.Last()); // Create return of the function
            var b = Expression.Block(typeof(object), result.Item1 , result.Item2);
            var lbd = Expression.Lambda<Func<TContext, object>>(b, arg0);
            return lbd;
        }


        public static (IEnumerable<ParameterExpression>, List<Expression>) GetAccessorToArgumentGenerateCode(string fullPath, ParameterExpression arg0)
        {

            Dictionary<Type, ParameterExpression> varDic = new Dictionary<Type, ParameterExpression>();
            List<Expression> blk = new List<Expression>();
            ParameterExpression currentInstance = arg0;
            Type lastInstanceType = currentInstance.Type;

            varDic.Add(currentInstance.Type, currentInstance);

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
                        varDic.Add(typeof(DynObject), (p = Expression.Parameter(typeof(DynObject), "_" + typeof(DynObject).Name.ToLower())));

                    if (!varDic.TryGetValue(typeof(string), out parameterResult))
                        varDic.Add(typeof(string), (parameterResult = Expression.Parameter(typeof(string), "_" + typeof(string).Name.ToLower())));

                    var method = lastInstanceType.GetMethod("GetWithPath", new Type[] { typeof(Queue<string>) });
                    var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, currentInstance));
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
                        var j = Expression.Assign(parameterResult, Expression.Call(p, method, _p, currentInstance));
                        blk.Add(j);

                    }
                    else
                    {

                        if (!varDic.TryGetValue(property.PropertyType, out parameterResult))
                            varDic.Add(property.PropertyType, (parameterResult = Expression.Parameter(property.PropertyType, "_" + property.PropertyType.Name.ToLower())));

                        var e = Expression.Assign(parameterResult, Expression.Property(p, property));
                        blk.Add(e);

                        var i = Expression.IfThen(Expression.Equal(parameterResult, Expression.Constant(null)),
                            typeof(NullReferenceException).Throw(Expression.Constant(memberName))
                            );
                        blk.Add(i);

                        lastInstanceType = property.PropertyType;
                    }
                }

            }


            return (varDic.Values, blk);

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
        private static bool LogResult(string ruleName, bool result, TContext context, string[] names, object[] arguments)
        {

            if (FunctionalLog == null)
            {
                StringBuilder sb = new StringBuilder(1000);
                sb.Append(ruleName);
                sb.Append(" (");
                string comma = string.Empty;
                for (int i = 0; i < names.Length; i++)
                {
                    sb.Append(comma);
                    var a = arguments[i];
                    sb.Append(names[i]);
                    sb.Append(" : ");
                    sb.Append(a == null ? "null" : a.GetType().Name);
                    sb.Append(" = ");
                    sb.Append(a);
                    comma = ", ";
                }
                sb.Append(") =>");
                sb.Append(result ? "'true'" : "'false'");
                Trace.WriteLine(sb.ToString());
            }
            else
                FunctionalLog(ruleName, result, context, names, arguments);

            return result;
        }

        public static Func<string, bool, TContext, string[], object[], bool> FunctionalLog;


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
