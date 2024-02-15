using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bb.VisualStudio.Parser.Workflows.Grammar
{

    public class WorkflowConfigParsers
    {
        private readonly TextWriter _output;
        private readonly TextWriter _outputError;

        public WorkflowConfigParsers(TextWriter output = null, TextWriter outputError = null)
        {
            this.Errors = new List<ErrorModel>();
            this._output = output;
            this._outputError = outputError;
        }

        public ResultParsing ParseString(StringBuilder source, string sourceFile)
        {

            ResultParsing resultParsing = null;

            using (var l = Lock(sourceFile))
            {

                var a = this.Errors.Where(c => c.Filename == sourceFile).ToList();
                foreach (var item in a)
                    this.Errors.Remove(item);

                var result = WorkflowConfigParser.ParseString(source, sourceFile, this._output, this._outputError);
                var visitor = (WorkflowConfigVisitor)new WorkflowConfigVisitor()
                {
                    Filename = sourceFile,
                }.Visit(result.Tree)
                    ;

                this.Errors.AddRange(visitor.Errors);

                resultParsing = new ResultParsing()
                {
                    Keywords = visitor.Keywords,
                    Texts = visitor.Texts,
                    References = visitor.References,
                };

            }

            return resultParsing;

        }

        public void ParsePath(string source)
        {

            var payload = ContentHelper.LoadContentFromFile(source);
            ParseString(payload, source);

        }

        private Dictionary<string, BoxWorkflowConfigParser> _dic = new Dictionary<string, BoxWorkflowConfigParser>();

        public List<ErrorModel> Errors { get; private set; }


        private Disposable Lock(string sourceFile)
        {

            if (!_dic.TryGetValue(sourceFile, out BoxWorkflowConfigParser box))
                lock (_lock)
                    if (!_dic.TryGetValue(sourceFile, out box))
                        _dic.Add(sourceFile, box = new BoxWorkflowConfigParser() { SourceFile = sourceFile });

            var d = new Disposable(box);

            return d;

        }

        private class Disposable : IDisposable
        {

            public BoxWorkflowConfigParser Box { get; }

            private string sourceFile;

            public Disposable(BoxWorkflowConfigParser box)
            {
                this.Box = box;
                this.sourceFile = box.SourceFile;
                this.Box.Lock();
            }

            public void Dispose()
            {
                this.Box.Unlock();
            }


        }

        private volatile object _lock = new object();

    }

}
