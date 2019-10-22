using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bb.Workflows.Expresssions
{

    public class Statement
    {

        public Expression Expression { get; set; }

        public virtual Expression GetExpression(HashSet<string> variableParent)
        {

            Expression expression = this.Expression;

            if (expression.CanReduce)
                expression = expression.Reduce();

            return expression;

        }

    }

}
