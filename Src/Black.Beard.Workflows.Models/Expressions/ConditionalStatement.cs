using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Bb.Workflows.Expresssions
{

    public class ConditionalStatement : Statement
    {

        public SourceCode Then { get; set; }

        public SourceCode Else { get; set; }

        public override Expression GetExpression(HashSet<string> variableParent)
        {

            Expression b1;
            Expression b2 = null;


            if (Then.Count == 1)
                b1 = Then[0].Expression;
            else
                b1 = Then.GetExpression(new HashSet<string>(variableParent));

            if (Else != null)
            {
                if (Else.Count == 1)
                    b2 = Else[0].Expression;
                else
                    b2 = Else.GetExpression(new HashSet<string>(variableParent));
            }

            Expression expression = b2 == null
                ? Expression.IfThen(Expression, b1)
                : Expression.IfThenElse(Expression, b1, b2)
                ;

            if (expression.CanReduce)
                expression = expression.Reduce();

            return expression;

        }

    }

}
