﻿using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bb.Workflows.Expresssions
{
    public class ExpressionStatement : Statement
    {

        public Expression Expression { get; set; }

        public override Expression GetExpression(HashSet<string> variableParent)
        {

            Expression expression = this.Expression;

            if (expression.CanReduce)
                expression = expression.Reduce();

            return expression;

        }

    }

}
