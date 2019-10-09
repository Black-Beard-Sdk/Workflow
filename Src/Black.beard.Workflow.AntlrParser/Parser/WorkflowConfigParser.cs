using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Bb.Workflows.Models.Configurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bb.Workflows.Parser
{


    public static class WorkflowsConfigLoader
    {


        public static void Load(WorkflowsConfig config, Func<WorkflowConfigVisitor> visitorCreator, string path, string searchPattern, TextWriter output = null, TextWriter outputError = null)
        {

            if (visitorCreator == null)
                throw new NullReferenceException(nameof(visitorCreator));

            var items = ParseDirectory(path, searchPattern, output, outputError)
                .ToList();

            var fragments = items.Where(c => c.IsFragment).ToDictionary(c => c.File);

            foreach (var parser in items.Where(c => !c.IsFragment))
            {
                var visitor = visitorCreator();

                if (parser.Includes.Any())
                    foreach (var include in parser.Includes)
                        if (!fragments.TryGetValue(include, out WorkflowConfigParser p2))
                            throw new System.IO.FileNotFoundException($"missing file {include}");
                        else
                        {
                            visitor.Filename = p2.File;
                            visitor.Visit(p2.Tree);
                        }

                visitor.Filename = parser.File;
                var document = (WorkflowConfig)visitor.Visit(parser.Tree);

                config.AddDocument(document);

            }

        }

        /// <summary>
        /// Parse directory and load a parser for every document
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchPattern"></param>
        /// <param name="output"></param>
        /// <param name="outputError"></param>
        /// <returns></returns>
        private static IEnumerable<WorkflowConfigParser> ParseDirectory(string path, string searchPattern, TextWriter output = null, TextWriter outputError = null)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            dir.Refresh();
            foreach (var item in dir.GetFiles(searchPattern))
            {
                yield return WorkflowConfigParser.ParsePath(item.FullName, output, outputError);
            }
        }


    }

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
                Crc = Crc32.Calculate(source),
            };
            parser.ParseCharStream(stream);
            return parser;

        }

        /// <summary>
        /// Load specified document in a dedicated parser
        /// </summary>
        /// <param name="source"></param>
        /// <param name="output"></param>
        /// <param name="outputError"></param>
        /// <returns></returns>
        public static WorkflowConfigParser ParsePath(string source, TextWriter output = null, TextWriter outputError = null)
        {

            var payload = LoadContent(source);
            ICharStream stream = CharStreams.fromstring(payload.ToString());

            var parser = new WorkflowConfigParser(output, outputError)
            {
                File = source,
                Content = payload,
                Crc = Crc32.Calculate(payload),
            };

            parser.ParseCharStream(stream);

            return parser;

        }

        /// <summary>
        /// Loads the content of the file.
        /// </summary>
        /// <param name="rootSource">The root source.</param>
        /// <returns></returns>
        public static StringBuilder LoadContent(string rootSource)
        {
            StringBuilder result = new StringBuilder(WorkflowContentHelper.LoadContentFromFile(rootSource));
            return result;
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

            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Trace.WriteLine(this.File);

            var context = this._context;
            return visitor.Visit(context);

        }

        public bool InError { get => this._parser.ErrorListeners.Count > 0; }
        public uint Crc { get; private set; }

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
    }


    public interface IFile
    {

        string Filename { get; set; }

    }

}
