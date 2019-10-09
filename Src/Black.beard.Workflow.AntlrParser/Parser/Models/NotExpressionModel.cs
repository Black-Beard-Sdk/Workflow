using System.Text;

namespace Bb.Workflows.Parser.Models
{

    /// <summary>
    /// not expression
    /// </summary>
    public class NotExpressionModel : ExpressionModel
    {
        /// <summary>
        /// Expression to inverse result
        /// </summary>
        public ExpressionModel Expression { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"NOT {Expression.ToString()}");

            return sb.ToString();
        }


        /// <summary>
        /// Pattern visitor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitNot(this);
        }


    }

}