using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bb.Workflows.Expresssions
{
    public class GotoStatement : Statement
    {

        public Label Label { get; set; }


        public override Expression GetExpression(HashSet<string> variableParent)
        {
            return null;
        }


    }

}
