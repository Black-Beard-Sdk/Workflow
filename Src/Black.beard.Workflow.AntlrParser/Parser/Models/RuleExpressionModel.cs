using System;
using System.Collections.Generic;
using System.Text;

namespace Bb.Workflows.Parser.Models
{
    /// <summary>
    /// Reference to rule by name
    /// </summary>
    public class RuleExpressionModel : ExpressionModel
    {

        /// <summary>
        /// Reference rule name
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Refenrece at the rule definition
        /// </summary>
        public RuleDefinitionModel Reference { get; set; }

        public Dictionary<string, string> Arguments { get; } = new Dictionary<string, string>();

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitRuleExpression(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Key);

            return sb.ToString();
        }

    }

}