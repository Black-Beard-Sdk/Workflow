using System.Linq.Expressions;

namespace Bb.Workflows.Expresssions
{
    public class Label
    {

        public LabelTarget Instance { get; set; }

        public string Name { get; internal set; }

        public KindLabelEnum Kind { get; internal set; }

    }

    public enum KindLabelEnum
    {

        Default,
        Break,
        Continue,
        Return,

    }

}
