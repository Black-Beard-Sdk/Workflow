using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Bb.VisualStudio.Parser.Workflows.Grammar
{

    public class WorkflowConfigVisitor : WorkflowParserBaseVisitor<object>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        public WorkflowConfigVisitor(CultureInfo culture = null)
        {
            this._currentCulture = culture ?? CultureInfo.InvariantCulture;
        }

        public override object Visit(IParseTree tree)
        {
            this._keywords = new List<KeywordModel>();
            this._errors = new List<ErrorModel>();
            this._references = new List<ReferenceModel>();
            this._texts = new List<TextModel>();
            this._dic = new Dictionary<Type, List<BoxRuleContext>>();
            this._hash = new HashSet<Type>()
            {
                typeof(WorkflowParser.ConstantNameDefinitionContext),
                typeof(WorkflowParser.StateNameDefinitionContext),
                typeof(WorkflowParser.StateNamereferenceContext),
                typeof(WorkflowParser.EventNamereferenceContext),
                typeof(WorkflowParser.EventNameDefinitionContext),
                typeof(WorkflowParser.ActionNameDefinitionContext),
                typeof(WorkflowParser.ActionNameReferenceContext),
                typeof(WorkflowParser.RuleNameDefinitionContext),
                typeof(WorkflowParser.RuleNameReferenceContext),
            };

            EvaluateSyntaxErrors(tree);

            EvaluateSemanticErrors();

            return this;

        }

        public void EvaluateSyntaxErrors(IParseTree item)
        {

            if (item != null)
            {

                if (item is ErrorNodeImpl e)
                    this._errors.Add(new ErrorModel()
                    {
                        Filename = Filename,
                        Line = e.Symbol.Line,
                        StartIndex = e.Symbol.StartIndex,
                        Column = e.Symbol.Column,
                        Text = e.Symbol.Text,
                        Message = $"Failed to parse script at position {e.Symbol.StartIndex}, line {e.Symbol.Line}, col {e.Symbol.Column} '{e.Symbol.Text}'",
                        Code = "Syntax"
                    });

                else if (item is ParserRuleContext r)
                {
                    if (_hash.Contains(item.GetType()))
                    {
                        Add(r);
                        this._references.Add(new ReferenceModel()
                        {
                            Filename = Filename,
                            Line = r.Start.Line,
                            StartIndex = r.Start.StartIndex,
                            Column = r.Start.Column,
                            Text = r.Start.Text,
                        });
                    }
                }

                else if (item is ITerminalNode t)
                {
                    this._keywords.Add(new KeywordModel()
                    {
                        Filename = Filename,
                        Line = t.Symbol.Line,
                        StartIndex = t.Symbol.StartIndex,
                        Column = t.Symbol.Column,
                        Text = t.Symbol.Text,
                    });
                }

                int c = item.ChildCount;
                for (int i = 0; i < c; i++)
                    EvaluateSyntaxErrors(item.GetChild(i));

            }

        }

        private void EvaluateSemanticErrors()
        {

            Check(typeof(WorkflowParser.StateNamereferenceContext), typeof(WorkflowParser.StateNameDefinitionContext));
            Check(typeof(WorkflowParser.EventNamereferenceContext), typeof(WorkflowParser.EventNameDefinitionContext));
            Check(typeof(WorkflowParser.ActionNameReferenceContext), typeof(WorkflowParser.ActionNameDefinitionContext));
            Check(typeof(WorkflowParser.RuleNameReferenceContext), typeof(WorkflowParser.RuleNameDefinitionContext));

            //if (this._dic.TryGetValue(typeRef, out List<BoxRuleContext> listRef))
            //{
            //    if (!this._dic.TryGetValue(typeDef, out List<BoxRuleContext> listDef))
            //        listDef = new List<BoxRuleContext>();
            //    Check(listRef, listDef);
            //}

        }

        private void Check(Type typeRef, Type typeDef)
        {
            if (this._dic.TryGetValue(typeRef, out List<BoxRuleContext> listRef))
            {
                this._dic.TryGetValue(typeDef, out List<BoxRuleContext> listDef);
                Check(listRef, listDef);
            }
        }

        private void Check(List<BoxRuleContext> listRef, List<BoxRuleContext> listDef)
        {

            if (listDef != null)
            {
                var list = listDef.ToList();

                foreach (var item in list)
                    if (listDef.Count(c => c.Text == item.Text) > 1)
                        this._errors.Add(new ErrorModel()
                        {
                            Filename = Filename,
                            Line = item.Start.Line,
                            StartIndex = item.Start.StartIndex,
                            Column = item.Start.Column,
                            Text = item.Text,
                            Message = $"duplicated {item.Type} '{item.Text}' at position {item.Start.StartIndex}, line {item.Start.Line}, column {item.Start.Column}",
                            Code = "Semantic",
                        });

            }

            foreach (var item in listRef)
                if (!listDef.Any(c => c.Text == item.Text))
                    this._errors.Add(new ErrorModel()
                    {
                        Filename = Filename,
                        Line = item.Start.Line,
                        StartIndex = item.Start.StartIndex,
                        Column = item.Start.Column,
                        Text = item.Text,
                        Message = $"missing {item.Type} '{item.Text}' at position {item.Start.StartIndex}, line {item.Start.Line}, column {item.Start.Column}",
                        Code = "Semantic",
                    });

        }

        /// <summary>
        /// script_fragment | script_full
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitScript([NotNull] WorkflowParser.ScriptContext context)
        {
            return base.VisitScript(context);
        }

        /// <summary>
        /// script_fragment :
        ///     FRAGMENT NAME key
        ///     (DESCRIPTION comment)?
	    ///     (define_referenciel_statement SEMICOLON)+ 
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitScript_fragment([NotNull] WorkflowParser.Script_fragmentContext context)
        {
            return base.VisitScript_fragment(context);
        }

        /// <summary>
        /// script_full :
        ///     (INCLUDE CHAR_STRING) *
        ///     NAME key (VERSION versionNumber=number)?
        ///     (CONCURENCY concurencyNumber=number)?
        ///     (DESCRIPTION comment)?
        ///     (MATCHING matchings)?
        ///     (define_referenciel_statement SEMICOLON)+ 
        ///     initializing?
        ///     (define_state_statement SEMICOLON) *
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitScript_full([NotNull] WorkflowParser.Script_fullContext context)
        {
            return base.VisitScript_full(context);
        }

        public override object VisitMatching([NotNull] WorkflowParser.MatchingContext context)
        {
            return base.VisitMatching(context);
        }

        /// <summary>
        /// constant_declaration :
        ///     CONST key value comment?
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitConstant_declaration([NotNull] WorkflowParser.Constant_declarationContext context)
        {
            return base.VisitConstant_declaration(context);
        }


        /// <summary>
        /// state :
        ///     STATE StateNameDefinition comment?
        ///     (ON (ENTER STATE | EXIT STATE | ENTER AND EXIT STATE) execute)*
        ///     on_event_statement*
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitState([NotNull] WorkflowParser.StateContext context)
        {

            return base.VisitState(context);
        }

        /// <summary>
        /// INITIALIZE WORKFLOW initializing_item+
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitInitializing([NotNull] WorkflowParser.InitializingContext context)
        {
            return base.VisitInitializing(context);
        }

        /// <summary>
        /// initializing_item :
        ///     ON EVENT eventNamereference (WHEN rule_conditions)? RECURSIVE? SWITCH key
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitInitializing_item([NotNull] WorkflowParser.Initializing_itemContext context)
        {
            return base.VisitInitializing_item(context);
        }

        /// <summary>
        /// on_event_statement :
        ///    (ON EVENT key | EXPIRE AFTER delay) switch_state+
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitOn_event_statement([NotNull] WorkflowParser.On_event_statementContext context)
        {
            return base.VisitOn_event_statement(context);
        }

        /// <summary>
        /// switch_state :
        ///     (WHEN rule_conditions)?
        ///     (
        ///           execute2* SWITCH key
        ///         | execute2+
        ///     )
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitSwitch_state([NotNull] WorkflowParser.Switch_stateContext context)
        {
            return base.VisitSwitch_state(context);
        }

        /// <summary>
        /// execute :
        ///     ON(ENTER STATE | EXIT STATE | ENTER AND EXIT STATE)
        ///     (WHEN rule_conditions)?
        ///     (WAITING delay BEFORE)? 
        ///     (
        ///           EXECUTE action+ (SET matchings)?
        ///         | SET matchings
        ///     )
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitExecute([NotNull] WorkflowParser.ExecuteContext context)
        {
            return base.VisitExecute(context);
        }

        /// <summary>
        /// execute2 :
        ///     (WHEN rule_conditions)? 
        ///     (WAITING delay BEFORE)? 
        ///     execute3+
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitExecute2([NotNull] WorkflowParser.Execute2Context context)
        {
            return base.VisitExecute2(context);
        }

        /// <summary>
        /// execute3 :
        ///      (EXECUTE | key) action+
        ///    | STORE matchings+
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitExecute3([NotNull] WorkflowParser.Execute3Context context)
        {
            return base.VisitExecute3(context);

        }

        /// <summary>
        /// action : 
        ///     key LEFT_PAREN arguments? RIGHT_PAREN
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitAction([NotNull] WorkflowParser.ActionContext context)
        {
            return base.VisitAction(context);
        }

        /// <summary>
        /// arguments : 
        ///     argument(COMMA argument)*
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitArguments([NotNull] WorkflowParser.ArgumentsContext context)
        {
            return base.VisitArguments(context);
        }

        /// <summary>
        /// argument :
        ///     string EQUAL argumentValue
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitArgument([NotNull] WorkflowParser.ArgumentContext context)
        {
            return base.VisitArgument(context);
        }

        /// <summary>
        /// string | keys
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitArgumentValue([NotNull] WorkflowParser.ArgumentValueContext context)
        {
            return base.VisitArgumentValue(context);
        }

        /// <summary>
        /// rule_conditions :
        ///       key LEFT_PAREN arguments? RIGHT_PAREN
        ///     | NOT rule_conditions
        ///     | rule_conditions AND rule_conditions
        ///     | rule_conditions OR rule_conditions
        ///     | LEFT_PAREN rule_conditions RIGHT_PAREN
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitRule_conditions([NotNull] WorkflowParser.Rule_conditionsContext context)
        {
            return base.VisitRule_conditions(context);
        }

        /// <summary>
        /// event_declaration_statement :
        ///     EVENT key comment?
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitEvent_declaration_statement([NotNull] WorkflowParser.Event_declaration_statementContext context)
        {
            return base.VisitEvent_declaration_statement(context);
        }

        /// <summary>
        /// action_declaration_statement :
        ///     ACTION key LEFT_PAREN parameters? RIGHT_PAREN comment?
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitAction_declaration_statement([NotNull] WorkflowParser.Action_declaration_statementContext context)
        {
            return base.VisitAction_declaration_statement(context);
        }

        /// <summary>
        /// rule_declaration_statement :
        ///     RULE key LEFT_PAREN parameters? RIGHT_PAREN comment?
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitRule_declaration_statement([NotNull] WorkflowParser.Rule_declaration_statementContext context)
        {
            return base.VisitRule_declaration_statement(context);

        }

        /// <summary>
        /// parameters : 
        ///     parameter(COMMA parameter)*
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitParameters([NotNull] WorkflowParser.ParametersContext context)
        {
            return base.VisitParameters(context);
        }

        /// <summary>
        /// parameter :
        ///     type key
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitParameter([NotNull] WorkflowParser.ParameterContext context)
        {
            return base.VisitParameter(context);
        }

        /// <summary>
        /// type : 
        ///     TEXT | INTEGER | DECIMAL
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitType([NotNull] WorkflowParser.TypeContext context)
        {
            return base.VisitType(context);
        }

        /// <summary>
        /// number : NUMBER;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitNumber([NotNull] WorkflowParser.NumberContext context)
        {
            return base.VisitNumber(context);
        }

        /// <summary>
        /// key : REGULAR_ID;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitKey([NotNull] WorkflowParser.KeyContext context)
        {
            return base.VisitKey(context);
        }

        /// <summary>
        /// AROBASE? key (DOT key)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitCompositekey([NotNull] WorkflowParser.CompositekeyContext context)
        {
            return base.VisitCompositekey(context);
        }

        /// <summary>
        /// comment : CHAR_STRING;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitComment([NotNull] WorkflowParser.CommentContext context)
        {
            return base.VisitComment(context);
        }

        /// <summary>
        /// numeric :
        ///     number DOT number
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitNumeric([NotNull] WorkflowParser.NumericContext context)
        {
            return base.VisitNumeric(context);
        }

        /// <summary>
        /// string :
        ///     CHAR_STRING
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitString([NotNull] WorkflowParser.StringContext context)
        {
            return base.VisitString(context);
        }

        /// <summary>
        /// delay :
        ///     number (MINUTE | HOUR | DAY)
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitDelay([NotNull] WorkflowParser.DelayContext context)
        {
            return base.VisitDelay(context);
        }




        public IEnumerable<ErrorModel> Errors { get => this._errors; }

        public IEnumerable<KeywordModel> Keywords { get => this._keywords; }

        public IEnumerable<ReferenceModel> References { get => this._references; }

        public IEnumerable<TextModel> Texts { get => this._texts; }

        public string Filename { get; set; }

        public uint Crc { get; set; }

        [System.Diagnostics.DebuggerStepThrough]
        [System.Diagnostics.DebuggerNonUserCode]
        private void Stop()
        {
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }

        private StringBuilder GetText(RuleContext context)
        {

            if (context is ParserRuleContext s)
                return GetText(s.Start.StartIndex, s.Stop.StopIndex + 1);
            return new StringBuilder();
        }

        private StringBuilder GetText(int startIndex, int stopIndex)
        {

            int length = stopIndex - startIndex;

            length++;

            StringBuilder sb2 = new StringBuilder(length);
            char[] ar = new char[length];
            _initialSource.CopyTo(startIndex, ar, 0, length);
            sb2.Append(ar);

            return sb2;

        }

        private void Add(ParserRuleContext node)
        {

            var n = node.GetType();

            if (!this._dic.TryGetValue(n, out List<BoxRuleContext> list))
                this._dic.Add(n, list = new List<BoxRuleContext>());

            list.Add(new BoxRuleContext(node));

        }

        private class BoxRuleContext
        {

            public BoxRuleContext(ParserRuleContext node)
            {
                this._node = node;
                this.Text = this._node.GetText();
                this.Start = this._node.Start;
                this.Stop = this._node.Stop;

                var n = node.GetType().Name;
                this.Type = n.Substring(0, n.Length - "Context".Length);

            }

            private readonly ParserRuleContext _node;

            public T Node<T>()
                where T : ParserRuleContext
            {
                return (T)_node;
            }

            public string Text { get; }

            public IToken Start { get; }

            public IToken Stop { get; }
            public string Type { get; }
        }

        private StringBuilder _initialSource;
        private Dictionary<Type, List<BoxRuleContext>> _dic;
        private HashSet<Type> _hash;

        private List<ErrorModel> _errors;
        private List<KeywordModel> _keywords;
        private List<ReferenceModel> _references;
        private List<TextModel> _texts;

        private readonly CultureInfo _currentCulture;

    }

}
