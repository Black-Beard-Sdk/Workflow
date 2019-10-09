using System.Linq.Expressions;

namespace Bb.Workflows.Parser.Models
{

    public class ConstantExpressionModel : DefinitionModel
    {

        public ConstantExpressionModel()
        {

        }

        public object Value { get; set; }


        /// <summary>
        /// Pattern visitor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitConstant(this);
        }


    }

}