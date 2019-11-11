using System.Threading;

namespace Bb.VisualStudio.Parser.Workflows.Grammar
{
    public class BoxWorkflowConfigParser
    {
        public string SourceFile { get; internal set; }



        public void Lock()
        {
            while (Interlocked.CompareExchange(ref locked, 1, 0) != 0)
                continue; // spin
        }

        public void Unlock()
        {
            locked = 0;
        }

        private int locked;

    }

}
