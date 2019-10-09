namespace Bb.Workflows.Parser.Models
{

    /// <summary>
    /// Definition model base
    /// </summary>
    public class DefinitionModel
    {

        public string Key { get; set; }

        public string Comment { get; set; }

        public override string ToString()
        {
            return $"{Key} -> '{Comment}'";
        }

        ///// <summary>
        ///// Pattern visitor
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="visitor"></param>
        ///// <returns></returns>
        //public virtual T Accept<T>(IVisitor<T> visitor)
        //{
        //    return visitor.VisitDefinitionModel(this);
        //}

    }

}
