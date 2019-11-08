using Bb.Expresssions;
using Bb.Workflows.Expresssions;
using Bb.Workflows.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bb.Workflows.Parser.Models
{

    public class StateConverterVisitor<TContext> : IVisitor<Expression>
        where TContext : RunContext
    {

        public StateConverterVisitor(Dictionary<string, ConstantExpressionModel> constants)
        {

            _GetName = (t) => "_" + t.Name.ToLower();

            _constants = constants;

            this._context = this._block.AddParameter(typeof(TContext), _GetName(typeof(TContext)));
            this._resultVariable = _block.AddVar(typeof(bool), "_result" + _GetName(typeof(bool)));

            this._block.Assign(this._resultVariable, true.AsConstant());

        }


        public (Func<TContext, bool>, string) Visit(ExpressionModel e)
        {

            var result = e.Accept(this);
            _block.Add(result);

            var lbd = _block.GenerateLambda<Func<TContext, bool>>();

            return (lbd.Compile(), lbd.ToString());

        }

        public Expression VisitConstant(ConstantExpressionModel m)
        {
            return Expression.Constant(m);
        }

        public Expression VisitBinary(BinaryExpressionModel m)
        {

            var resultVariable = _resultVariable;


            _resultVariable = _block.AddVar(typeof(bool));
            var left = m.Left.Accept(this);

            _resultVariable = _block.AddVar(typeof(bool));
            var right = m.Right.Accept(this);

            BinaryExpression result = null;
            switch (m.Operator)
            {
                case "AND":
                    result = Expression.And(left, right);
                    break;

                case "OR":
                    result = Expression.Or(left, right);
                    break;

                default:
                    break;
            }


            _resultVariable = resultVariable;
            _block.Assign(this._resultVariable, result);
            _block.Add(GetCallLogAction(m.Operator, this._resultVariable));

            Debug.Assert(result != null);

            return _resultVariable;

        }

        public Expression VisitRule(RuleDefinitionModel m)
        {
            return null;
        }

        public Expression VisitNot(NotExpressionModel m)
        {

            var e = m.Expression.Accept(this);
            var not = Expression.Not(e);
            _block.Assign(this._resultVariable, not);

            _block.Add(GetCallLogAction("NOT", this._resultVariable));

            return this._resultVariable;

        }

        public Expression VisitRuleExpression(RuleExpressionModel m)
        {

            List<Expression> arguments = new List<Expression>()
            {
                this._context
            };

            BuildArguments(m, arguments);

            var catchBlk = new SourceCode()
            {
                
            };
            var p1 = catchBlk.AddVar(typeof(Exception));
                        
            this._block.Try
                (
                    new SourceCode()
                        .Assign(this._resultVariable, m.Reference.Method.Call(arguments.ToArray()))
                        .Add(GetCallLogAction(m.Reference.Method, m.Key, arguments.ToArray())),
                    
                    new CatchStatement()
                    {
                        Parameter = p1,
                        Body = catchBlk
                            .Add(GetCallLogActionException(m.Reference.Method, m.Key, p1, arguments.ToArray()))
                            .ReThrow()
                    }

                );

            return this._resultVariable;

        }

        private void BuildArguments(RuleExpressionModel m, List<Expression> arguments)
        {
            var parameterMethodList = m.Reference.Method.GetParameters().Skip(1).ToArray();

            for (int i = 0; i < parameterMethodList.Length; i++)
            {

                var item = parameterMethodList[i];
                if (!m.Arguments.TryGetValue(item.Name, out string value))
                    throw new Exceptions.MissingArgumentNameMethodReferenceException($"missing argument {item.Name} in {m.Reference.Method.Name}");

                ConstantExpression constant = GetConstants(item.Name, value, item.ParameterType);
                if (constant != null)
                    arguments.Add(constant);

                else
                {
                    BuildGetAccessorToArgumentGenerateCode(value.Substring(1));
                    arguments.Add(_block.LastVariable.ConvertIfDifferent(item.ParameterType));
                }
            }
        }

        /// <summary>
        /// Return an expression from the method
        /// </summary>
        /// <param name="argumentContext"></param>
        /// <returns></returns>
        public Expression GetCallLogAction(MethodInfo methodLog, string rulename, params Expression[] arguments)
        {

            // build custom method
            List<Expression> _args = new List<Expression>(arguments.Length);
            var parameters = methodLog.GetParameters()
                .ToArray();

            // Build log method
            List<Expression> _argValues = new List<Expression>();
            List<Expression> _argNames = new List<Expression>();

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                _argNames.Add(Expression.Constant(parameters[i].Name));
                _argValues.Add(argument.ConvertIfDifferent(typeof(object)));
            }

            var call = BusinessLog<TContext>.MethodLogResult.Call(
                rulename.AsConstant(),
                this._resultVariable,
                this._context,
                typeof(string).NewArray(_argNames),
                typeof(object).NewArray(_argValues)
            );

            return call;

        }

        /// <summary>
        /// Return an expression from the method
        /// </summary>
        /// <param name="argumentContext"></param>
        /// <returns></returns>
        public Expression GetCallLogActionException(MethodInfo method, string rulename, ParameterExpression parameter, params Expression[] arguments)
        {

            // build custom method
            List<Expression> _args = new List<Expression>(arguments.Length);
            var parameters = method.GetParameters()
                .ToArray();

            // Build log method
            List<Expression> _argValues = new List<Expression>();
            List<Expression> _argNames = new List<Expression>();

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];
                _argNames.Add(Expression.Constant(parameters[i].Name));
                _argValues.Add(argument.ConvertIfDifferent(typeof(object)));
            }

            var call = BusinessLog<TContext>.MethodLogResultException.Call(
                rulename.AsConstant(),
                parameter,
                this._context,
                typeof(string).NewArray(_argNames),
                typeof(object).NewArray(_argValues)
            );

            return call;

        }

        /// <summary>
        /// Return an expression from the method
        /// </summary>
        /// <param name="argumentContext"></param>
        /// <returns></returns>
        public Expression GetCallLogAction(string rulename, params Expression[] arguments)
        {

            // build custom method
            List<Expression> _args = new List<Expression>(arguments.Length);

            // Build log method
            List<Expression> _argValues = new List<Expression>();
            List<Expression> _argNames = new List<Expression>();

            for (int i = 0; i < arguments.Length; i++)
            {
                var argument = arguments[i];

                if (argument is ParameterExpression p)
                {
                    _argNames.Add(Expression.Constant(p.Name));
                    _argValues.Add(argument.ConvertIfDifferent(typeof(object)));
                }
                else
                    throw new InvalidOperationException("only expresion of type ParameterExpression are managed");
            }

            var call = BusinessLog<TContext>.MethodLogResult.Call(
                rulename.AsConstant(),
                this._resultVariable,
                this._context,
                typeof(string).NewArray(_argNames),
                typeof(object).NewArray(_argValues)
            );

            return call;

        }

        public void BuildGetAccessorToArgumentGenerateCode(string fullPath)
        {

            List<Expression> blk = new List<Expression>();
            ParameterExpression currentInstance = this._context;
            Type lastInstanceType = currentInstance.Type;

            var path = new Queue<string>(fullPath.Split('.'));
            PropertyInfo property;
            ParameterExpression parameterResult = null;


            while (path.Count > 0)
            {
                var memberName = path.Dequeue();
                if (lastInstanceType == typeof(DynObject))
                {

                    var _p = GetPath(path, memberName);

                    var p = _block.AddVarIfNotExists(typeof(DynObject), _GetName(typeof(DynObject)));
                    parameterResult = _block.AddVarIfNotExists(typeof(string), _GetName(typeof(string)));

                    var method = lastInstanceType.GetMethod("GetWithPath", new Type[] { typeof(Queue<string>), typeof(TContext), typeof(string) });
                    _block.Assign(parameterResult, p.Call(method, _p, currentInstance, Expression.Constant(fullPath)));

                }
                else
                {

                    var p = _block.AddVarIfNotExists(lastInstanceType, _GetName(lastInstanceType));

                    property = lastInstanceType.GetProperty(memberName);
                    if (property == null)
                    {

                        var _last = p;
                        var _p = GetPath(path, memberName);
                        p = _block.AddVarIfNotExists(typeof(DynObject), _GetName(typeof(DynObject)));

                        var ExtendedDatasMethod = _last.Type.GetMethod("ExtendedDatas");
                        _block.Assign(p, Expression.Call(_last, ExtendedDatasMethod));

                        parameterResult = _block.AddVarIfNotExists(typeof(string), _GetName(typeof(string)));
                        var method = typeof(DynObject).GetMethod("GetWithPath", new Type[] { typeof(Queue<string>), typeof(TContext), typeof(string) });
                        _block.Assign(parameterResult, p.Call(method, _p, currentInstance, Expression.Constant(fullPath)));

                    }
                    else
                    {

                        parameterResult = _block.AddVarIfNotExists(property.PropertyType, _GetName(property.PropertyType));
                        _block.Assign(parameterResult, Expression.Property(p, property));

                        string msgError = $"key {memberName} is missing in {fullPath}";
                        _block.If(Expression.Equal(parameterResult, Expression.Constant(null))
                            , typeof(NullReferenceException).Throw(Expression.Constant(msgError))
                            );

                        lastInstanceType = property.PropertyType;

                    }
                }

            }

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

        private ConstantExpression GetConstants(string key, string value, Type type)
        {

            ConstantExpression constant = null;

            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                value = value.Trim('\'');
                var v = Convert.ChangeType(value, type);
                constant = Expression.Constant(v);
            }
            else if (value.StartsWith("@"))
                constant = null;
            //func = BusinessAction<TContext>.GetAccessorToArgument(value.Substring(1), this._context);

            else
            {

                if (this._constants.TryGetValue(value, out ConstantExpressionModel m))
                {
                    var v = Convert.ChangeType(m.Value, type);
                    constant = Expression.Constant(v);
                }
                else
                {
                    var v = Convert.ChangeType(value, type);
                    constant = Expression.Constant(v);
                }
            }

            return constant;

        }


        private List<ParameterExpression> _variables = new List<ParameterExpression>();
        private SourceCodeMethod _block = new SourceCodeMethod();
        private readonly Func<Type, string> _GetName;
        private readonly Dictionary<string, ConstantExpressionModel> _constants;
        private ParameterExpression _context;
        private ParameterExpression _resultVariable;
    }

    //public static class MethodDiscovery
    //{

    //    /// <summary>
    //    /// Permet de retourner la liste des methodes d'evaluation disponibles dans les types fournis.
    //    /// </summary>
    //    /// <param name="types"></param>
    //    /// <returns></returns>
    //    public static Dictionary<string, BusinessAction<T>> GetActions<T>(IBusinessMethodDiscovery methodDiscovery, bool startWith, Type returnType, params Type[] MethodSign) //where T : Context
    //    {

    //        if (returnType == null)
    //            throw new ArgumentNullException(nameof(returnType));

    //        var result = new Dictionary<string, BusinessAction<T>>();

    //        foreach (var action in methodDiscovery.GetActions<T>(returnType, MethodSign))
    //        {
    //            if (result.ContainsKey(action.RuleName))
    //                Trace.WriteLine($"duplicate rule '{action.RuleName}' in {action.Origin}");
    //            else
    //                result.Add(action.RuleName, action);
    //        }

    //        return result;

    //    }

    //}

    ///// <summary>
    ///// Permet de retourner la liste des methodes d'evaluation disponibles dans les types fournis.
    ///// </summary>
    //public interface IBusinessMethodDiscovery
    //{

    //    /// <summary>
    //    /// Return list of method for the specified arguments
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="returnType"></param>
    //    /// <param name="methodSign"></param>
    //    /// <returns></returns>
    //    /// <exception cref="ArgumentNullException">returnType</exception>
    //    /// <exception cref="ArgumentNullException">methodSign</exception>
    //    IEnumerable<BusinessAction<T>> GetActions<T>(Type returnType, params Type[] methodSign); //where T : Context;

    //}

}
