using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Bb.Workflows.Models;
using Bb.Workflows.Models.Configurations;
using Bb.Workflows.Exceptions;
using Bb.Workflows.Parser.Models;
using System.Reflection;
using System.Linq.Expressions;
using Bb.Workflows.Expresssions;

namespace Bb.Workflows.Parser
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

        public void EvaluateErrors(IParseTree item)
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
                        Message = $"Failed to parse script at position {e.Symbol.StartIndex}, line {e.Symbol.Line}, col {e.Symbol.Column} ' {e.Symbol.Text}'"
                    });

                int c = item.ChildCount;
                for (int i = 0; i < c; i++)
                {
                    IParseTree child = item.GetChild(i);
                    EvaluateErrors(child);
                }

            }

        }

        public override object Visit(IParseTree tree)
        {
            this._errors = new List<ErrorModel>();
            EvaluateErrors(tree);
            if (this._errors.Count > 0)
                Stop();
            return base.Visit(tree);

        }

        /// <summary>
        /// script_fragment | script_full
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitScript([NotNull] WorkflowParser.ScriptContext context)
        {

            this._initialSource = new StringBuilder(context.Start.InputStream.ToString());

            if (this._workflow == null)
                this._workflow = new WorkflowConfig()
                {
                    Crc = this.Crc,
                };
            else
                this._workflow.Crc ^= this._workflow.Crc;

            base.VisitScript(context);

            this._workflow.BuildWorkflow();

            return this._workflow;

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

            if (context.DESCRIPTION() != null)
                this._workflow.Label = (string)VisitComment(context.comment());

            var define_referenciel_statements = context.define_referenciel_statement();
            foreach (var define_referenciel_statement in define_referenciel_statements)
            {
                var declaration = VisitDefine_referenciel_statement(define_referenciel_statement);
                switch (declaration)
                {

                    case ConstantExpressionModel c:
                        this._constants.Add(c.Key, c);
                        break;

                    case DeclaredEventConfig e:
                        this._workflow.DeclaredEvents.Add(e.Name, e);
                        break;

                    case MethodReference r:         // Do nothing
                        break;

                    default:
                        throw new NotImplementedException(declaration.GetType().Name);


                }

            }

            Stop();

            return null;

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

            this._workflow.Name = (string)VisitKey(context.key());

            if (context.VERSION() != null)
                this._workflow.Version = (int)VisitNumber(context.versionNumber);
            else
                this._workflow.Version = 1;

            var concurency = context.concurency();
            if (concurency != null)
                this._workflow.Concurrency = (int)VisitConcurency(concurency);
            else
                this._workflow.Concurrency = 1;

            if (context.DESCRIPTION() != null)
                this._workflow.Label = (string)VisitComment(context.comment());

            if (context.MATCHING() != null)
            {
                var filters = (List<(string, string)>)VisitMatchings(context.matchings());
                foreach (var matching in filters)
                    this._workflow.AddFilter(matching.Item1, matching.Item2);
            }

            var define_referenciel_statements = context.define_referenciel_statement();
            foreach (var define_referenciel_statement in define_referenciel_statements)
            {
                var declaration = VisitDefine_referenciel_statement(define_referenciel_statement);
                switch (declaration)
                {

                    case ConstantExpressionModel c:
                        this._constants.Add(c.Key, c);
                        break;

                    case DeclaredEventConfig e:
                        this._workflow.DeclaredEvents.Add(e.Name, e);
                        break;

                    case MethodReference r:         // Do nothing
                        break;

                    default:
                        throw new NotImplementedException(declaration.GetType().Name);


                }

            }

            // Add states definitions
            var define_state_statements = context.define_state_statement();
            if (define_state_statements != null)
                foreach (var define_state_statement in define_state_statements)
                {
                    var declaration = VisitDefine_state_statement(define_state_statement);
                    if (declaration is StateConfig s)
                        this._workflow.AddState(s);

                    else
                    {
                        Stop();
                        throw new NotImplementedException(declaration.GetType().Name);
                    }
                }

            // Add initializers
            var initializing = context.initializing();
            if (initializing != null)
            {
                var inits = (List<InitializationOnEventConfig>)VisitInitializing(initializing);
                foreach (var init in inits)
                    this._workflow.AddInitializer(init);
            }


            // workflow.Errors.AddRange(this._errors);

            return null;

        }

        public override object VisitConcurency([NotNull] WorkflowParser.ConcurencyContext context)
        {
            return VisitNumber(context.number());
        }

        /// <summary>
        /// matchings : 
        ///     LEFT_PAREN matching+ RIGHT_PAREN
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitMatchings([NotNull] WorkflowParser.MatchingsContext context)
        {
            List<(string, string)> _result = new List<(string, string)>();
            var args = context.matching();
            foreach (var item in args)
            {
                var arg = ((string, string))VisitMatching(item);
                _result.Add(arg);
            }
            return _result;
        }

        public override object VisitMatching([NotNull] WorkflowParser.MatchingContext context)
        {
            return ((string)VisitKey(context.key()), (string)VisitString(context.@string()));
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
            var key = (string)VisitKey(context.key());
            var value = VisitValue(context.value());
            string comment = string.Empty;
            var c = context.comment();
            if (c != null)
                comment = (string)Visit(c);
            return new ConstantExpressionModel() { Key = key, Value = value, Comment = comment };
        }

        /// <summary>
        /// state :
        ///     STATE key comment?
        ///     (ON (ENTER STATE | EXIT STATE | ENTER AND EXIT STATE) execute)*
        ///     on_event_statement*
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitState([NotNull] WorkflowParser.StateContext context)
        {

            string comment = string.Empty;
            var c = context.comment();
            if (c != null)
                comment = (string)Visit(c);

            StateConfig state = new StateConfig()
            {
                Name = (string)VisitKey(context.key()),
                Label = comment,
            };

            var executes = context.execute();
            if (executes != null)
                foreach (var execute in executes)
                {

                    var e = ((bool, bool, ResultRuleConfig))VisitExecute(execute);

                    if (e.Item1)
                        state.IncomingRules.Add(e.Item3);

                    if (e.Item2)
                        state.OutcomingRules.Add(e.Item3);

                }


            var on_event_statements = context.on_event_statement();
            if (on_event_statements != null)
                foreach (var on_event_statement in on_event_statements)
                {
                    var e = ((IncomingEventConfig, List<(string, ResultRuleConfig)>))VisitOn_event_statement(on_event_statement);
                    if (state.Events.TryGetValue(e.Item1.Name, out IncomingEventConfig ic))
                        throw new Exceptions.DuplicatedArgumentNameMethodReferenceException($"event name {e.Item1.Name} in {state.Name}");
                    state.Events.Add(e.Item1.Name, e.Item1);

                    foreach (var item in e.Item2)
                        switch (item.Item1)
                        {

                            case "in":
                                state.IncomingRules.Add(item.Item2);
                                break;

                            case "out":
                                state.OutcomingRules.Add(item.Item2);
                                break;

                            default:
                                break;
                        }

                }

            return state;
        }

        /// <summary>
        /// INITIALIZE WORKFLOW initializing_item+
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitInitializing([NotNull] WorkflowParser.InitializingContext context)
        {
            List<InitializationOnEventConfig> _result = new List<InitializationOnEventConfig>();
            var items = context.initializing_item();
            foreach (var item in items)
                _result.Add((InitializationOnEventConfig)VisitInitializing_item(item));
            return _result;
        }

        /// <summary>
        /// initializing_item :
        ///     ON EVENT key (WHEN rule_conditions)? RECURSIVE? SWITCH key
        ///     ;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitInitializing_item([NotNull] WorkflowParser.Initializing_itemContext context)
        {

            var keys = context.key();
            string eventName = (string)VisitKey(keys[0]);
            string _switch = (string)VisitKey(keys[1]);

            string conditionTxt = string.Empty;
            string conditionCode = string.Empty;
            Func<RunContext, bool> func = null;
            RuleSpan whenRulePosition = null;
            if (context.WHEN() != null)
            {
                var condition = context.rule_conditions();
                conditionTxt = condition.GetText();
                var compilation = ((Func<RunContext, bool>, string))Compile((ExpressionModel)VisitRule_conditions(condition));
                func = compilation.Item1;
                conditionCode = compilation.Item2;
                whenRulePosition = new RuleSpan()
                {
                    StartLine = condition.Start.Line,
                    StartColumn = condition.Start.Column,
                    StartIndex = condition.Start.StartIndex,

                    StopLine = condition.Stop.Line,
                    StopColumn = condition.Stop.Column,
                    StopIndex = condition.Stop.StopIndex,
                };
            }

            InitializationOnEventConfig c1 = new InitializationOnEventConfig()
            {
                EventName = eventName,
                Recursive = context.RECURSIVE() != null,
            }
            .AddSwitch(_switch, func, conditionTxt, conditionCode, whenRulePosition);

            return c1;
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

            List<(string, ResultRuleConfig)> resultActions = new List<(string, ResultRuleConfig)>();
            Func<RunContext, bool> whenRule = null;
            string whenRuleText = string.Empty;
            var key = context.key();

            IncomingEventConfig incomingEvent = new IncomingEventConfig();

            if (key != null)
                incomingEvent.Name = (string)VisitKey(key);

            if (context.EXPIRE() != null)
            {

                incomingEvent.Name = Constants.Events.ExpiredEventName;
                int delay = (int)VisitDelay(context.delay());
                whenRule = (ctx) =>
                {
                    var i = ctx.IncomingEvent.ExtendedDatas["CurrentState"];
                    if (i.GetValue == null)
                        return false;
                    var j = i.GetValue(ctx)?.ToString();
                    return ctx.Workflow.CurrentState == j;
                };
                whenRuleText = $"@IncomingEvent.CurrentState == @Workflow.CurrentState";

                AddExpirationActions(resultActions, delay);

            }

            InsertTransitions(context, whenRule, incomingEvent, whenRuleText);

            return (incomingEvent, resultActions);

        }

        private void InsertTransitions(WorkflowParser.On_event_statementContext context, Func<RunContext, bool> whenRule, IncomingEventConfig incomingEvent, string ruleText)
        {
            var switchs = context.switch_state();
            foreach (var item in switchs)
            {
                var s = (TransitionConfig)VisitSwitch_state(item);
                if (whenRule != null)
                {
                    if (s.WhenRule == null)
                    {
                        s.WhenRule = whenRule;
                        s.WhenRuleText = ruleText;
                    }
                    else
                    {
                        var r = s.WhenRule;
                        s.WhenRule = (ctx) => whenRule(ctx) && r(ctx);
                        s.WhenRuleText += " && " + ruleText;
                    }
                }
                incomingEvent.AddTransition(s);
            }
        }

        private static void AddExpirationActions(List<(string, ResultRuleConfig)> resultActions, int delay)
        {

            string uuidKey = Guid.NewGuid().ToString();

            ResultActionConfig resultAction;
            ResultRuleConfig rule;

            #region action to add to 'On Enter State' (expiration event) 

            resultAction = new ResultActionConfig()
            {
                Name = Constants.Events.ExpiredEventName,
                Label = "Expiration",
                Delay = delay,
                Kind = Constants.PushActionName
            };

            rule = new ResultRuleConfig()
            {
            };
            rule.Actions.Add(resultAction);
            resultActions.Add(("in", rule));

            #endregion

            #region action to add to 'On Exit State' (remove expiration event)

            resultAction = new ResultActionConfig()
            {
                Name = Constants.Events.CancelReminderAction,
                Label = "Cancel Expiration",
                Kind = Constants.PushActionName,
            };

            rule = new ResultRuleConfig()
            {
                WhenRuleText = $"@IncomingEvent.Name != '{Constants.Events.ExpiredEventName}'",
                WhenRule = (ctx) => ctx.IncomingEvent.Name != Constants.Events.ExpiredEventName,
            };
            rule.Actions.Add(resultAction);
            resultActions.Add(("out", rule));

            #endregion

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

            var t = new TransitionConfig()
            {

            };

            if (context.WHEN() != null)
            {
                var condition = context.rule_conditions();
                t.WhenRuleText = condition.GetText();
                t.WhenRulePosition = new RuleSpan()
                {
                    StartLine = condition.Start.Line,
                    StartColumn = condition.Start.Column,
                    StartIndex = condition.Start.StartIndex,

                    StopLine = condition.Stop.Line,
                    StopColumn = condition.Stop.Column,
                    StopIndex = condition.Stop.StopIndex,
                };
                var compilation = ((Func<RunContext, bool>, string))Compile((ExpressionModel)VisitRule_conditions(condition));
                t.WhenRule = compilation.Item1;
                t.WhenRuleCode = compilation.Item2;
            }

            var executes = context.execute2();
            if (executes != null)
                foreach (var execute in executes)
                    t.RuleActions.Add((ResultRuleConfig)VisitExecute2(execute));

            if (context.SWITCH() != null)
                t.TargetStateName = (string)VisitKey(context.key());

            return t;
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
            bool @in = context.ENTER() != null;
            bool @out = context.EXIT() != null;
            var r = (ResultRuleConfig)VisitExecute2(context.execute2());
            return (@in, @out, r);
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

            ResultRuleConfig rule = new ResultRuleConfig()
            {

            };

            int delay = 0;
            if (context.WAITING() != null)
                delay = (int)VisitDelay(context.delay());


            if (context.WHEN() != null)
            {
                var condition = context.rule_conditions();
                rule.WhenRuleText = condition.GetText();
                rule.WhenRulePosition = new RuleSpan()
                {
                    StartLine = condition.Start.Line,
                    StartColumn = condition.Start.Column,
                    StartIndex = condition.Start.StartIndex,

                    StopLine = condition.Stop.Line,
                    StopColumn = condition.Stop.Column,
                    StopIndex = condition.Stop.StopIndex,
                };

                var compilation = ((Func<RunContext, bool>, string))Compile((ExpressionModel)VisitRule_conditions(condition));
                rule.WhenRule = compilation.Item1;
                rule.WhenRuleCode = compilation.Item2;
            }

            var a = context.execute3();

            foreach (var item1 in a)
            {
                var _a = (List<ResultActionConfig>)VisitExecute3(item1);
                foreach (var item2 in _a)
                {
                    item2.Delay = delay;
                    rule.Actions.Add(item2);
                }
            }

            return rule;

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

            List<ResultActionConfig> _actions = new List<ResultActionConfig>();

            if (context.STORE() != null)
            {

                var matchings = context.matchings();
                foreach (var item_matching in matchings)
                {

                    var _stores = (List<(string, string)>)VisitMatchings(item_matching);

                    foreach (var item in _stores)
                    {

                        var act = new ResultActionConfig()
                        {
                            Name = "update " + item.Item1,
                            Label = $"update {item.Item1} to {item.Item2}",
                            Kind = Constants.SetValueActionName,
                        }
                        .AddArgument("key", item.Item1)
                        .AddArgument("value", item.Item2)
                        ;

                        _actions.Add(act);

                    }

                }
            }
            else
            {

                string kind = context.EXECUTE() != null
                    ? Constants.PushActionName
                    : (string)VisitKey(context.key())
                ;

                var actions = context.action();
                foreach (var action in actions)
                {
                    var act = (ResultActionConfig)VisitAction(action);
                    act.Kind = kind;
                    _actions.Add(act);
                }

            }

            return _actions;

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

            var act = new ResultActionConfig()
            {
                Name = (string)VisitKey(context.key()),
            };

            if (!this._actions.TryGetValue(act.Name, out MethodReference method))
                throw new InvalidMethodReferenceException(act.Name);

            Dictionary<string, Type> o = method.Arguments.ToList().ToDictionary(c => c.Key, c => c.Value);

            var args = context.arguments();
            if (args != null)
            {
                var a = (List<(string, string)>)VisitArguments(args);
                foreach ((string, string) item in a)
                {
                    var type = ResolveTypeOfArgument(item.Item1, method);
                    var func = GetLambda(item.Item1, item.Item2, type);

                    act.Arguments.Add(item.Item1, func);
                    if (!o.ContainsKey(item.Item1))
                        if (!method.Arguments.ContainsKey(item.Item1))
                            throw new DuplicatedArgumentNameMethodReferenceException(item.Item1);
                    o.Remove(item.Item1);
                }

                if (o.Count > 0)
                    throw new MissingArgumentNameMethodReferenceException(string.Join(", ", o));

            }

            return act;
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
            List<(string, string)> _list = new List<(string, string)>();
            var args = context.argument();
            foreach (var item in args)
            {
                var arg = ((string, string))VisitArgument(item);
                _list.Add(arg);
            }
            return _list;
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
            string key = (string)VisitKey(context.key());
            string value = (string)VisitArgumentValue(context.argumentValue());
            return (key, value);

        }

        /// <summary>
        /// string | keys
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitArgumentValue([NotNull] WorkflowParser.ArgumentValueContext context)
        {

            var key = context.compositekey();
            if (key != null)
                return VisitCompositekey(key);

            return context.@string().GetText();

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

            ExpressionModel result = null;
            var key = context.key();
            if (key != null)
            {

                var k = (string)VisitKey(key);
                var r1 = new RuleExpressionModel()
                {
                    Key = k,
                    Reference = ResolveRule(k),
                };

                var args = context.arguments();
                if (args != null)
                {
                    var arguments = (List<(string, string)>)VisitArguments(args);
                    foreach (var argument in arguments)
                        r1.Arguments.Add(argument.Item1, argument.Item2);
                }

                result = r1;
            }

            else if (context.NOT() != null)
                result = new NotExpressionModel()
                {
                    Expression = (ExpressionModel)VisitRule_conditions(context.rule_conditions()[0])
                };

            else if (context.AND() != null)
                result = new BinaryExpressionModel()
                {
                    Left = (ExpressionModel)VisitRule_conditions(context.rule_conditions()[0]),
                    Operator = "AND",
                    Right = (ExpressionModel)VisitRule_conditions(context.rule_conditions()[1])
                };

            else if (context.OR() != null)
                result = new BinaryExpressionModel()
                {
                    Left = (ExpressionModel)VisitRule_conditions(context.rule_conditions()[0]),
                    Operator = "OR",
                    Right = (ExpressionModel)VisitRule_conditions(context.rule_conditions()[1])
                };

            else if (context.LEFT_PAREN() != null && context.RIGHT_PAREN() != null)
                result = (ExpressionModel)VisitRule_conditions(context.rule_conditions()[0]);

            return result;

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
            string comment = string.Empty;
            var c = context.comment();
            if (c != null)
                comment = (string)Visit(c);
            return new DeclaredEventConfig() { Name = (string)VisitKey(context.key()), Label = comment };
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

            var key = (string)VisitKey(context.key());

            string comment = string.Empty;
            var c = context.comment();
            if (c != null)
                comment = (string)Visit(c);

            if (!this._actions.TryGetValue(key, out MethodReference r))
                this._actions.Add(key, r = new MethodReference() { Name = key, Comment = comment });
            else
                throw new Exceptions.DuplicatedArgumentNameMethodReferenceException("Action method name" + key);

            var p = context.parameters();
            if (p != null)
            {
                var h = (Dictionary<string, Type>)VisitParameters(p);
                foreach (var item in h)
                    r.Arguments.Add(item.Key, item.Value);
            }

            return r;


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

            var key = (string)VisitKey(context.key());

            string comment = string.Empty;
            var c = context.comment();
            if (c != null)
                comment = (string)Visit(c);

            if (!this._actions.TryGetValue(key, out MethodReference r))
                this._actions.Add(key,
                    r = new MethodReference()
                    {
                        Name = key,
                        Comment = comment
                    });
            else
                throw new Exceptions.DuplicatedArgumentNameMethodReferenceException("Rule method name" + key);

            var p = context.parameters();
            if (p != null)
            {
                var h = (Dictionary<string, Type>)VisitParameters(p);
                foreach (var item in h)
                    r.Arguments.Add(item.Key, item.Value);
            }

            return r;

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

            Dictionary<string, Type> _h = new Dictionary<string, Type>();
            foreach (var item in context.parameter())
            {
                var pp = ((Type, string))VisitParameter(item);
                if (!_h.ContainsKey(pp.Item2))
                    this._errors.Add(new ErrorModel()
                    {
                        StartIndex = item.Start.StartIndex,
                        Line = item.Start.Line,
                        Column = item.Start.Column,
                        Text = item.Start.Text,
                        Message = $"duplicated parameter {pp} at position {item.Start.StartIndex}, line {item.Start.Line}, col {item.Start.Column}"
                    });
                _h.Add(pp.Item2, pp.Item1);
            }
            return _h;
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
            return ((Type)VisitType(context.type()), (string)VisitKey(context.key()));
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

            if (context.TEXT() != null)
                return typeof(string);

            if (context.INTEGER() != null)
                return typeof(Int64);

            if (context.DECIMAL() != null)
                return typeof(double);

            return typeof(object);

        }

        /// <summary>
        /// number : NUMBER;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitNumber([NotNull] WorkflowParser.NumberContext context)
        {
            var text = context.GetText();

            var i = Int64.Parse(text, this._currentCulture);

            if (i <= Int32.MaxValue)
                return (Int32)i;

            return i;
        }

        /// <summary>
        /// key : REGULAR_ID;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitKey([NotNull] WorkflowParser.KeyContext context)
        {
            return context.GetText();
        }

        /// <summary>
        /// AROBASE? key (DOT key)
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitCompositekey([NotNull] WorkflowParser.CompositekeyContext context)
        {

            StringBuilder sb = new StringBuilder(100);

            var k = context.key();

            if (context.AROBASE() != null)
            {
                sb.Append("@");
            }

            string comma = string.Empty;
            foreach (var item in k)
            {
                sb.Append(comma);
                sb.Append((string)VisitKey(item));
                comma = ".";
            }

            return sb.ToString();

        }

        /// <summary>
        /// comment : CHAR_STRING;
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override object VisitComment([NotNull] WorkflowParser.CommentContext context)
        {
            return context.GetText().Trim().Trim('\'');
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
            Stop();
            var numerics = context.number();
            var l = Int64.Parse(numerics[0].GetText(), this._currentCulture);
            var r = Double.Parse("0" + numerics[1].GetText(), this._currentCulture);
            r = r + l;
            return r;
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
            return context.GetText().Trim().Trim('\'');
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
            int result = (int)VisitNumber(context.number());

            if (context.HOUR() != null)
                result *= 60;

            else if (context.DAY() != null)
                result *= 60 * 24;

            return result;
        }




        public IEnumerable<ErrorModel> Errors { get => this._errors; }

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

        public WorkflowConfigVisitor AddRule(string name, Func<RunContext, bool> function)
        {
            AddRule(name, function.Method);
            return this;
        }

        public WorkflowConfigVisitor AddRule(string name, MethodInfo method)
        {
            var r = new RuleDefinitionModel()
            {
                Key = name,
                Comment = string.Empty,
                Method = method,
            };
            _rules.Add(r.Key, r);
            return this;
        }

        private Type ResolveTypeOfArgument(string key, MethodReference method)
        {

            if (!method.Arguments.TryGetValue(key, out Type type))
                throw new InvalidArgumentNameMethodReferenceException(key);

            return type;

        }

        private Func<RunContext, object> GetLambda(string key, string value, Type type)
        {

            Func<RunContext, object> func = null;

            if (value.StartsWith("'") && value.EndsWith("'"))
            {
                value = value.Trim('\'');
                var v = Convert.ChangeType(value, type);
                func = ExpressionHelper.GetConstant<object>(v);
            }
            else if (value.StartsWith("'@"))
                func = ExpressionHelper.GetAccessors<RunContext>(value.Substring(1));

            else
            {

                if (this._constants.TryGetValue(value, out ConstantExpressionModel m))
                {
                    var v = Convert.ChangeType(m.Value, type);
                    func = ExpressionHelper.GetConstant<object>(v);
                }
                else
                {
                    var v = Convert.ChangeType(value, type);
                    func = ExpressionHelper.GetConstant<object>(v);
                }
            }

            return func;

        }

        private RuleDefinitionModel ResolveRule(string key)
        {

            if (!this._rules.TryGetValue(key, out RuleDefinitionModel rule))
                throw new Exceptions.InvalidMethodReferenceException(key);

            return rule;

        }

        private (Func<RunContext, bool>, string) Compile(ExpressionModel expressionModel)
        {
            StateConverterVisitor<RunContext> visitor = new StateConverterVisitor<RunContext>(_constants);
            (Func<RunContext, bool>, string) result = visitor.Visit(expressionModel);
            return result;
        }


        private readonly Dictionary<string, ConstantExpressionModel> _constants = new Dictionary<string, ConstantExpressionModel>();
        private readonly Dictionary<string, RuleDefinitionModel> _rules = new Dictionary<string, RuleDefinitionModel>();
        private readonly Dictionary<string, MethodReference> _actions = new Dictionary<string, MethodReference>();

        private StringBuilder _initialSource;
        private WorkflowConfig _workflow;
        private List<ErrorModel> _errors;
        private readonly CultureInfo _currentCulture;

    }

}
