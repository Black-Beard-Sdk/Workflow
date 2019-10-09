using System.Text;

namespace Bb.Workflows.Parser.Models
{

    /// <summary>
    /// Binary expression
    /// </summary>
    public class BinaryExpressionModel : ExpressionModel
    {

        /// <summary>
        /// Left expression
        /// </summary>
        public ExpressionModel Left { get; set; }

        /// <summary>
        /// Opeator name
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Right expression
        /// </summary>
        public ExpressionModel Right { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Left.ToString()} {Operator} {Right.ToString()}");

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
            return visitor.VisitBinary(this);
        }


    }

}