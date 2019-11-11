using System.Collections.Generic;

namespace Bb.VisualStudio.Parser.Workflows.Grammar
{
    public class ResultParsing
    {
        public IEnumerable<KeywordModel> Keywords { get; internal set; }
        public IEnumerable<TextModel> Texts { get; internal set; }
        public IEnumerable<ReferenceModel> References { get; internal set; }
    }

}
