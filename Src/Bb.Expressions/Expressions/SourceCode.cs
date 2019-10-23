﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Bb.Workflows.Expresssions
{
    public class SourceCode : List<Statement>
    {

        internal SourceCode(SourceCode parent = null)
        {
            this._parent = parent;
        }

        #region Labels 

        public Label AddLabel(string name = null, KindLabelEnum kind = KindLabelEnum.Default)
        {

            if (string.IsNullOrEmpty(name))
                name = this._variables.GetNewName();

            var instance = Expression.Label(Labels.GetNewName());
            var label = new Label() { Instance = instance, Kind = kind };
            this.AddLabel(label);

            return label;
        }

        public SourceCode AddLabel(Label label)
        {
            this._labels.Add(label);
            return this;
        }

        #endregion Labels 

        #region variables

        public ParameterExpression AddVarIfNotExists(Type type, string name)
        {

            var vari = this._variables.GetByName(name);
            if (vari != null)
                return vari.Instance;

            if (string.IsNullOrEmpty(name))
                name = this._variables.GetNewName();

            var instance = Expression.Parameter(type, name);
            this.AddVar(instance);

            return instance;

        }

        public SourceCode AddVarIfNotExists(ParameterExpression parameter)
        {

            var vari = this._variables.GetByName(parameter.Name);
            if (vari == null)
                this.AddVar(parameter);

            return this;

        }

        public ParameterExpression AddVar(Type type, string name = null)
        {

            if (string.IsNullOrEmpty(name))
                name = this._variables.GetNewName();

            var instance = Expression.Parameter(type, name);
            this.AddVar(instance);

            return instance;
        }

        public SourceCode AddVar(ParameterExpression arg)
        {

            var vari = this._variables.GetByName(arg.Name);
            if (vari != null)
            {
                if (vari.Instance != arg)
                    throw new Exception($"parameter {arg.Name} allready exists");
            }
            else
            {
                var variable = new Variable() { Name = arg.Name, Instance = arg };
                this._variables.Add(variable);
                this.LastVariable = arg;
            }

            return this;

        }

        public ParameterExpression GetVar(string name)
        {
            var variable = _variables.GetByName(name);
            return variable?.Instance;
        }

        #endregion variables

        #region Goto

        public SourceCode Break()
        {
            var label = this.GetLabelImpl(KindLabelEnum.Break);
            Add(new GotoStatement() { Label = label });
            return this;
        }

        public SourceCode Continue()
        {
            var label = this.GetLabelImpl(KindLabelEnum.Continue);
            Add(new GotoStatement() { Label = label });
            return this;

        }

        public SourceCode Return(Expression @return)
        {
            var label = this.GetLabelImpl(KindLabelEnum.Return);
            Add(new GotoStatement() { Label = label, Expression = @return, });
            return this;

        }

        private Label GetLabelImpl(KindLabelEnum kind)
        {

            Label label = this._labels.Items.FirstOrDefault(c => c.Kind == kind);

            if (label == null && _parent == null)
                label = _parent.GetLabelImpl(kind);

            if (label == null)
                throw new Exception($"no label of {kind.ToString()} defined");

            return label;

        }

        #endregion Goto

        public SourceCode If(Expression e, params Expression[] codes)
        {

            var _then = new SourceCode()
            {

            };
            _then.AddRange(codes.Select(c => new ExpressionStatement() { Expression = c }));

            this.If(e, _then, null);

            return this;
        }


        public SourceCode If(Expression e, SourceCode @then, SourceCode @else)
        {

            var n = new ConditionalStatement()
            {
                Expression = e,
                Then = @then,
                Else = @else
            };

            this.Add(n);

            return this;
        }


        public SourceCode Call(MethodInfo self, params Expression[] arguments)
        {
            Add(self.Call(arguments));
            return this;
        }


        public SourceCode NewObject(Type type, params Expression[] args)
        {
            Add(type.Create(args));
            return this;
        }


        #region Exceptions


        public SourceCode Throw(Type type, params Expression[] args)
        {
            Add(type.Throw(args));
            return this;
        }

        #endregion Exceptions


        public SourceCode Assign(Expression left, Expression right)
        {
            Add(left.AssignFrom(right));
            return this;
        }


        public SourceCode Add(Expression expression)
        {
            this.Add(new ExpressionStatement() { Expression = expression });
            return this;
        }

        public SourceCode AddRange(IEnumerable<Expression> expressions)
        {
            var s2 = expressions.Select(c => new ExpressionStatement() { Expression = c });
            base.AddRange(s2);
            return this;
        }

        public new SourceCode Add(Statement statement)
        {
            base.Add(statement);
            this.LastStatement = statement;
            return this;
        }

        public new SourceCode AddRange(IEnumerable<Statement> statements)
        {
            foreach (var statement in statements)
                this.Add(statement);

            return this;
        }

        internal Expression GetExpression(HashSet<string> variableParent)
        {
            Expression expression = null;

            if (this.Count == 1)
                expression = this[0].GetExpression(variableParent);

            else
                expression = GetBlock(variableParent);

            if (expression.CanReduce)
                expression = expression.Reduce();

            return expression;

        }

        public IEnumerable<ParameterExpression> Variables { get => this._variables.Items.Select(c => c.Instance); }
        public Statement LastStatement { get; private set; }
        public ParameterExpression LastVariable { get; private set; }

        private BlockExpression GetBlock(HashSet<string> variableParent)
        {

            ParameterExpression[] __variables = CleanVariables(variableParent);

            var __list = new List<Expression>(this.Count + 10);
            foreach (Statement statement in this)
                __list.Add(statement.GetExpression(variableParent));

            return Expression.Block(__variables, __list.ToArray());

        }

        protected ParameterExpression[] CleanVariables(HashSet<string> variableParent)
        {

            var v = this._variables.Items.ToList();
            foreach (var item in v)
                if (!(variableParent.Add(item.Name)))
                    this._variables.RemoveByName(item.Name);

            var __variables = this._variables.Items.Select(c => c.Instance).ToArray();

            return __variables;

        }

        protected Variables _variables = new Variables();
        protected Labels _labels = new Labels();
        internal SourceCode _parent;
    }

}