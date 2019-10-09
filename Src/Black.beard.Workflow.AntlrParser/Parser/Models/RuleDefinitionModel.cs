using System.Reflection;

namespace Bb.Workflows.Parser.Models
{


    /// <summary>
    /// Define a rule definition
    /// </summary>
    public class RuleDefinitionModel : DefinitionModel
    {
        public MethodInfo Method { get; set; }

        /// <summary>
        /// Pattern visitor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitRule(this);
        }

    }


}
