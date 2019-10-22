using Bb.Workflows.Expresssions;
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

        static BusinessAction()
        {

        }

        public BusinessAction()
        {

        }

        //public static SourceCodeMethod GetAccessorToArgument(string fullPath, ParameterExpression arg0)
        //{

        //    SourceCodeMethod block = new SourceCodeMethod()
        //        .AddParameter(arg0);

        //    GetAccessorToArgumentGenerateCode(block, fullPath, arg0);
        //    block.Add(block.LastVariable);
            
        //    return block;
        
        //}


        //public static void GetAccessorToArgumentGenerateCode(SourceCode code, string fullPath, ParameterExpression arg0)
        //{

        //    Func<Type, string> GetName = (t) => "_" + t.Name.ToLower();

        //    List<Expression> blk = new List<Expression>();
        //    ParameterExpression currentInstance = arg0;
        //    Type lastInstanceType = currentInstance.Type;

        //    code.AddVar(currentInstance.Type, GetName(currentInstance.Type));

        //    var path = new Queue<string>(fullPath.Split('.'));
        //    PropertyInfo property;
        //    ParameterExpression parameterResult = null;


        //    while (path.Count > 0)
        //    {
        //        var memberName = path.Dequeue();
        //        if (lastInstanceType == typeof(DynObject))
        //        {

        //            var _p = GetPath(path, memberName);

        //            var p = code.AddVarIfNotExists(typeof(DynObject), GetName(typeof(DynObject)));
        //            parameterResult = code.AddVarIfNotExists(typeof(string), GetName(typeof(string)));

        //            var method = lastInstanceType.GetMethod("GetWithPath", new Type[] { typeof(Queue<string>) });
        //            code.Assign(parameterResult, p.Call(method, _p, currentInstance));

        //        }
        //        else
        //        {

        //            var p = code.AddVarIfNotExists(lastInstanceType, GetName(lastInstanceType));

        //            property = lastInstanceType.GetProperty(memberName);
        //            if (property == null)
        //            {

        //                var _last = p;
        //                var _p = GetPath(path, memberName);
        //                p = code.AddVarIfNotExists(typeof(DynObject), GetName(typeof(DynObject)));
        //                code.Assign(p, Expression.Property(_last, "ExtendedDatas"));

        //                parameterResult = code.AddVarIfNotExists(typeof(string), GetName(typeof(string)));
        //                var method = typeof(DynObject).GetMethod("GetWithPath", new Type[] { typeof(Queue<string>) });
        //                code.Assign(parameterResult, p.Call(method, _p, currentInstance));

        //            }
        //            else
        //            {

        //                parameterResult = code.AddVarIfNotExists(property.PropertyType, GetName(property.PropertyType));
        //                code.Assign(parameterResult, Expression.Property(p, property));

        //                code.If(Expression.Equal(parameterResult, Expression.Constant(null))
        //                    , typeof(NullReferenceException).Throw(Expression.Constant(memberName))
        //                    );

        //                lastInstanceType = property.PropertyType;
                    
        //            }
        //        }

        //    }

        //}

        //private static Expression GetPath(Queue<string> path, string memberName)
        //{
        //    List<string> _l = new List<string>(path.Count + 1) { memberName };
        //    while (path.Count > 0)
        //        _l.Add(path.Dequeue());
        //    var ctor = typeof(Queue<string>).GetConstructor(new Type[] { typeof(IEnumerable<string>) });
        //    var arg = Expression.NewArrayInit(typeof(string), _l.Select(c => Expression.Constant(c)).ToArray());
        //    var a = Expression.New(ctor, arg);

        //    return a;

        //}

        ///// <summary>
        ///// Return an expression from the method
        ///// </summary>
        ///// <param name="argumentContext"></param>
        ///// <returns></returns>
        //public Expression GetLoadDatasAction(params Expression[] arguments)
        //{

        //    // build custom method
        //    List<Expression> _args = new List<Expression>(arguments.Length);
        //    var parameters = this.Method.GetParameters();

        //    for (int i = 0; i < arguments.Length; i++)
        //    {
        //        var argument = arguments[i];
        //        var parameter = parameters[i];

        //        if (argument.Type != parameter.ParameterType)
        //        {

        //            if (argument is ConstantExpression c)
        //                argument = Expression.Constant(Convert.ChangeType(c.Value, parameter.ParameterType));

        //            else
        //            {
        //                if (System.Diagnostics.Debugger.IsAttached)
        //                    System.Diagnostics.Debugger.Break();
        //                argument = Expression.Convert(argument, parameter.ParameterType);
        //            }

        //        }
        //        _args.Add(argument);
        //    }

        //    var m = Expression.Call(this.Method, _args.ToArray());


        //    return m;

        //}

        //public MethodInfo Method { get; internal set; }

    }

}

