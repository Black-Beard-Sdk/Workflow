namespace Bb.Workflows.Models.Configurations
{
    public class RuleSpan
    {

        public int StartLine { get; set; }
        
        public int StartColumn { get; set; }
        
        public int StartIndex { get; set; }
        
        public int StopLine { get; set; }
        
        public int StopColumn { get; set; }
        
        public int StopIndex { get; set; }

        public static RuleSpan None { get; } = new RuleSpan();

    }

}
