﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Bb.Workflows.Parser.Models
{

    public class StateConverterVisitor<TContext> : IVisitor<Expression>
    {

        public StateConverterVisitor(Dictionary<string, ConstantExpressionModel> constants)
        {
            _constants = constants;
            this._context = Expression.Parameter(typeof(TContext), "context");
        }


        public Func<TContext, bool> Visit(ExpressionModel e)
        {

            var result = e.Accept(this);

            _block.Add(result);

            if (this._variables.Contains(this._context))
                this._variables.Remove(this._context);

            BlockExpression blk = Expression.Block(this._variables, _block.ToArray());

            var lbd = Expression.Lambda<Func<TContext, bool>>(blk, _context);

            var resultLbd = lbd.Compile();

            return resultLbd;

        }

        public Expression VisitConstant(ConstantExpressionModel m)
        {

            return Expression.Constant(m);

        }

        public Expression VisitBinary(BinaryExpressionModel m)
        {

            BinaryExpression result = null;

            var left = m.Left.Accept(this);
            var right = m.Right.Accept(this);

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

            Debug.Assert(result != null);
            return result;

        }

        public Expression VisitRule(RuleDefinitionModel m)
        {

            //List<Expression> arguments = new List<Expression>(/*m.Arguments.Count + */1)
            //{
            //    this._context
            //};

            //foreach (var item in m.Method.GetParameters().Skip(1))
            //{
            //    var p = Expression.Parameter(item.ParameterType, item.Name);
            //    arguments.Add(p);
            //}

            //var e = _actions.GetCallAction(arguments.ToArray());



            //return e;

            return null;
        }

        public Expression VisitNot(NotExpressionModel m)
        {
            var e = m.Expression.Accept(this);
            return Expression.Not(e);
        }

        public Expression VisitRuleExpression(RuleExpressionModel m)
        {

            List<Expression> arguments = new List<Expression>()
            {
                this._context
            };

            var parameterMethodList = m.Reference.Method.GetParameters().Skip(1).ToArray();

            for (int i = 0; i < parameterMethodList.Length; i++)
            {

                var item = parameterMethodList[i];
                if (!m.Arguments.TryGetValue(item.Name, out string value))
                    throw new Exceptions.MissingArgumentNameMethodReferenceException($"missing argument {item.Name} in {m.Reference.Method.Name}");

                ConstantExpression constant = GetConstants(item.Name, value, item.ParameterType);
                if (constant !=  null)
                    arguments.Add(constant);

                else
                {

                    var r = BusinessAction<TContext>.GetAccessorToArgumentGenerateCode(value.Substring(1), this._context);
                    _variables.AddRange(r.Item1);
                    _block.AddRange(r.Item2);

                    //var p2 = Expression.Variable(item.ParameterType, item.Name);
                    //_block.Add(Expression.Assign(p2, r.Item1.Last().ConvertIfDifferent(p2.ResolveType())));

                    //_variables.Add(p2);
                    arguments.Add(r.Item1.Last().ConvertIfDifferent(item.ParameterType));
                }
            }

            var _action = new BusinessAction<TContext>()
            {
                Method = m.Reference.Method,
                RuleName = m.Key,
            };

            var e = _action.GetCallAction(arguments.ToArray());

            return e;

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
        private List<Expression> _block = new List<Expression>();


        private readonly Dictionary<string, ConstantExpressionModel> _constants;
        private ParameterExpression _context;

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
