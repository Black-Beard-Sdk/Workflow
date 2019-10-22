using System;
using System.Linq.Expressions;

namespace Bb.Workflows.Expresssions
{
    public class Variable
    {

        public string Name { get; set; }

        public Type Type { get; set; }

        public ParameterExpression Instance { get; internal set; }

    }

}
