namespace Bb.VisualStudio.Parser.Workflows.Grammar
{
    public class TokenModel
    {

        public TokenModel()
        {

        }

        public string Text { get; internal set; }

        public int Column { get; internal set; }

        public int StartIndex { get; internal set; }

        public int Line { get; internal set; }

        public string Filename { get; internal set; }

    }

}
