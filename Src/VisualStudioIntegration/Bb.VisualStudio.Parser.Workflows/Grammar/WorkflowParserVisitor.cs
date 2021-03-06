//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from WorkflowParser.g4 by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Bb.VisualStudio.Parser.Workflows.Grammar {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="WorkflowParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public interface IWorkflowParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.script"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitScript([NotNull] WorkflowParser.ScriptContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.script_fragment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitScript_fragment([NotNull] WorkflowParser.Script_fragmentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.script_full"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitScript_full([NotNull] WorkflowParser.Script_fullContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.concurency"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConcurency([NotNull] WorkflowParser.ConcurencyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.define_referenciel_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDefine_referenciel_statement([NotNull] WorkflowParser.Define_referenciel_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.define_state_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDefine_state_statement([NotNull] WorkflowParser.Define_state_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.constant_declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstant_declaration([NotNull] WorkflowParser.Constant_declarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.value"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitValue([NotNull] WorkflowParser.ValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.state"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitState([NotNull] WorkflowParser.StateContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.initializing"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInitializing([NotNull] WorkflowParser.InitializingContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.initializing_item"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInitializing_item([NotNull] WorkflowParser.Initializing_itemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.on_event_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOn_event_statement([NotNull] WorkflowParser.On_event_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.switch_state"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSwitch_state([NotNull] WorkflowParser.Switch_stateContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.execute"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExecute([NotNull] WorkflowParser.ExecuteContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.execute2"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExecute2([NotNull] WorkflowParser.Execute2Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.execute3"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExecute3([NotNull] WorkflowParser.Execute3Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.matchings"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMatchings([NotNull] WorkflowParser.MatchingsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.matching"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMatching([NotNull] WorkflowParser.MatchingContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.action"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAction([NotNull] WorkflowParser.ActionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.arguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArguments([NotNull] WorkflowParser.ArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgument([NotNull] WorkflowParser.ArgumentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.argumentValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgumentValue([NotNull] WorkflowParser.ArgumentValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.rule_conditions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRule_conditions([NotNull] WorkflowParser.Rule_conditionsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.event_declaration_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEvent_declaration_statement([NotNull] WorkflowParser.Event_declaration_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.action_declaration_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAction_declaration_statement([NotNull] WorkflowParser.Action_declaration_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.rule_declaration_statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRule_declaration_statement([NotNull] WorkflowParser.Rule_declaration_statementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameters([NotNull] WorkflowParser.ParametersContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameter([NotNull] WorkflowParser.ParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] WorkflowParser.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComment([NotNull] WorkflowParser.CommentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.number"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumber([NotNull] WorkflowParser.NumberContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.numeric"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumeric([NotNull] WorkflowParser.NumericContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.string"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitString([NotNull] WorkflowParser.StringContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.delay"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDelay([NotNull] WorkflowParser.DelayContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.workflowNameDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWorkflowNameDefinition([NotNull] WorkflowParser.WorkflowNameDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.constantNameDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConstantNameDefinition([NotNull] WorkflowParser.ConstantNameDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.eventNameDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEventNameDefinition([NotNull] WorkflowParser.EventNameDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.actionNameDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitActionNameDefinition([NotNull] WorkflowParser.ActionNameDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.ruleNameDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRuleNameDefinition([NotNull] WorkflowParser.RuleNameDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.stateNameDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStateNameDefinition([NotNull] WorkflowParser.StateNameDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.eventNamereference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEventNamereference([NotNull] WorkflowParser.EventNamereferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.stateNamereference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStateNamereference([NotNull] WorkflowParser.StateNamereferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.actionNameReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitActionNameReference([NotNull] WorkflowParser.ActionNameReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.ruleNameReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRuleNameReference([NotNull] WorkflowParser.RuleNameReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.matchingKey"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMatchingKey([NotNull] WorkflowParser.MatchingKeyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.parameterNameDefinition"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameterNameDefinition([NotNull] WorkflowParser.ParameterNameDefinitionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.parameterNamereference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameterNamereference([NotNull] WorkflowParser.ParameterNamereferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.compositekey"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCompositekey([NotNull] WorkflowParser.CompositekeyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="WorkflowParser.key"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitKey([NotNull] WorkflowParser.KeyContext context);
}
} // namespace Bb.VisualStudio.Parser.Workflows.Grammar
