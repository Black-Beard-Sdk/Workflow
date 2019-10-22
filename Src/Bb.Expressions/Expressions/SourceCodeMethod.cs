using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bb.Workflows.Expresssions
{
    public class SourceCodeMethod : SourceCode
    {

        public SourceCodeMethod()
        {

        }

        public ParameterExpression AddParameter(Type type, string name = null)
        {

            if (string.IsNullOrEmpty(name))
                name = this._parameters.GetNewName();

            var instance = Expression.Parameter(type, name);
            this.AddParameter(instance);

            return instance;
        }

        public SourceCodeMethod AddParameter(ParameterExpression arg)
        {

            var vari = this._parameters.GetByName(arg.Name);
            if (vari != null)
            {
                if (vari.Instance != arg)
                    throw new Exception($"parameter {arg.Name} allready exists");
            }
            else
            {
                var variable = new Variable() { Name = arg.Name, Instance = arg };
                this._parameters.Add(variable);
                this.LastParameter = arg;
                if (this.GetVar(arg.Name) == null)
                    this.AddVar(arg.Type, arg.Name);
            }

            return this;

        }

        public ParameterExpression GetParameter(string name)
        {
            var variable = _parameters.GetByName(name);
            return variable.Instance;
        }

        public Expression<TDelegate> GenerateLambda<TDelegate>()
        {

            var parameters = this._parameters.Items.Select(c => c.Instance).ToArray();
            HashSet<string> variableParent = new HashSet<string>(parameters.Select(c => c.Name));

            var expression = this.GetExpression(variableParent);

            if (expression.CanReduce)
                expression = expression.Reduce();

            var result = Expression.Lambda<TDelegate>(expression, parameters.ToArray());

            return result;

        }

        public TDelegate Compile<TDelegate>()
        {

            //var asm = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("foo"), System.Reflection.Emit.AssemblyBuilderAccess.RunAndSave);
            //var mod = asm.DefineDynamicModule("mymod", true);
            //var type = mod.DefineType("baz", TypeAttributes.Public);
            //var meth = type.DefineMethod("go", MethodAttributes.Public | MethodAttributes.Static);
            //var sdi = Expression.SymbolDocument("");
            //var info = Expression.DebugInfo(sdi, 0, 0, 1, 1);

            var d1 = DebugInfoGenerator.CreatePdbGenerator()
                ;

            var lbd = GenerateLambda<TDelegate>()
                .Compile(d1);
                ;

            return lbd;

        }


        public LambdaExpression GenerateLambda(Type delegateType)
        {

            var parameters = this._parameters.Items.Select(c => c.Instance).ToArray();
            HashSet<string> variableParent = new HashSet<string>(parameters.Select(c => c.Name));

            var expression = this.GetExpression(variableParent);

            if (expression.CanReduce)
                expression = expression.Reduce();

            var result = Expression.Lambda(delegateType, expression, parameters.ToArray());

            return result;

        }

        public IEnumerable<ParameterExpression> Parameters { get => this._parameters.Items.Select(c => c.Instance); }
        public ParameterExpression LastParameter { get; private set; }

        private Variables _parameters = new Variables();

    }

}
