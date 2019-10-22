using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bb.Workflows.Expresssions
{

    public abstract class Statement
    {


        public abstract Expression GetExpression(HashSet<string> variableParent);

    }

}
