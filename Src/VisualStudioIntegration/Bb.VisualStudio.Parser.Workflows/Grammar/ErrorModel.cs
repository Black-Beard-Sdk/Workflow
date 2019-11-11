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



    public class ErrorModel : TokenModel
    {

        public ErrorModel()
        {

        }

        public string Code { get; set; }
        
        public string Message { get; set; }

        public override string ToString()
        {
            return Message.ToString();
        }

        public override int GetHashCode()
        {
            return Filename.GetHashCode() ^ Message.GetHashCode() ^ Text.GetHashCode() ^ (StartIndex).GetHashCode();
        }

    }

    public class KeywordModel : TokenModel
    {

        public KeywordModel()
        {

        }

    }

}
