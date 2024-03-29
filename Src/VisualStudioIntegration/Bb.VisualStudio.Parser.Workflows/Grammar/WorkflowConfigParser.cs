﻿using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Bb.VisualStudio.Parser.Workflows.Grammar;

namespace Bb.VisualStudio.Parser.Workflows.Grammar
{

    public class WorkflowConfigParser
    {

        private WorkflowConfigParser(TextWriter output, TextWriter outputError)
        {
            this.Output = output ?? Console.Out;
            this.OutputError = outputError ?? Console.Error;
            this._includes = new HashSet<string>();
        }

        public static WorkflowConfigParser ParseString(StringBuilder source, string sourceFile = "", TextWriter output = null, TextWriter outputError = null)
        {
            ICharStream stream = CharStreams.fromstring(source.ToString());

            var parser = new WorkflowConfigParser(output, outputError)
            {
                File = sourceFile ?? string.Empty,
                Content = source,
                Crc = source.ToString().GetHashCode(),
            };

            parser.ParseCharStream(stream);
            return parser;

        }

        public static bool Trace { get; set; }

        public WorkflowParser.ScriptContext Tree { get { return this._context; } }

        public IEnumerable<string> Includes { get => this._includes; }

        public string File { get; set; }

        public StringBuilder Content { get; private set; }

        public TextWriter Output { get; private set; }

        public TextWriter OutputError { get; private set; }

        private readonly HashSet<string> _includes;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Visit<Result>(IParseTreeVisitor<Result> visitor)
        {

            if (visitor is IFile f)
                f.Filename = this.File;

            var context = this._context;
            return visitor.Visit(context);

        }

        public bool InError { get => this._parser.ErrorListeners.Count > 0; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ParseCharStream(ICharStream stream)
        {

            var lexer = new WorkflowLexer(stream, this.Output, this.OutputError);
            var token = new CommonTokenStream(lexer);
            this._parser = new WorkflowParser(token)
            {
                BuildParseTree = true,
                //Trace = ScriptParser.Trace, // Ca plante sur un null, pourquoi ?
            };

            _context = _parser.script();

            this.IsFragment = _context.script_fragment() != null;
            if (!this.IsFragment)
            {
                var full = _context.script_full();
                if (full.INCLUDE() != null && full.INCLUDE().Length > 0)
                    foreach (var item in full.CHAR_STRING())
                        this._includes.Add(item.GetText().Trim('\''));
            }

        }

        private WorkflowParser _parser;
        private WorkflowParser.ScriptContext _context;
        public bool IsFragment { get; private set; }
        public int Crc { get; private set; }
    }

}
