using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bb.VisualStudio.Parser.Workflows
{


    public static class WorkflowTokens
    {

        static WorkflowTokens()
        {

            var lines = System.Text.Encoding.UTF8.GetString(TokenResources.WorkflowLexer)
                .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                ;

            WorkflowTokens.Keywords = new HashSet<string>();
            foreach (var line in lines)
            {
                var txt = line.Split('=')[0];
                txt = txt.Substring(1, txt.Length - 2);
                if (char.IsLetter(txt[0]))
                    Keywords.Add(txt);
            }

        }

        public static HashSet<string> Keywords { get; }
    
    }
}
