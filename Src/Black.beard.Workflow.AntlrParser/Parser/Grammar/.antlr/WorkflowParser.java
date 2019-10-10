// Generated from f:\Src\Sdk\Workflow\Src\Black.beard.Workflow.AntlrParser\Parser\Grammar\WorkflowParser.g4 by ANTLR 4.7.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast"})
public class WorkflowParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.7.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		ACTION=1, AFTER=2, AND=3, BEFORE=4, CONST=5, CONCURENCY=6, DESCRIPTION=7, 
		DAY=8, DEFINE=9, DECIMAL=10, EVENT=11, EXECUTE=12, ENTER=13, EQUAL=14, 
		EXIT=15, EXPIRE=16, FRAGMENT=17, HOUR=18, INCLUDE=19, INITIALIZE=20, INTEGER=21, 
		KEY=22, MATCHING=23, MINUTE=24, NAME=25, NOT=26, ON=27, OR=28, PARAMETER=29, 
		RULE=30, SAFE=31, SWITCH=32, STATE=33, STORE=34, TEXT=35, TIME=36, THREAD=37, 
		VERSION=38, WAITING=39, WITH=40, WHEN=41, WORKFLOW=42, CHAR_STRING=43, 
		LEFT_PAREN=44, RIGHT_PAREN=45, SEMICOLON=46, COLON=47, COMMA=48, DOT=49, 
		AROBASE=50, SPACES=51, NUMBER=52, SINGLE_LINE_COMMENT=53, MULTI_LINE_COMMENT=54, 
		REGULAR_ID=55;
	public static final int
		RULE_script = 0, RULE_script_fragment = 1, RULE_script_full = 2, RULE_concurency = 3, 
		RULE_define_referenciel_statement = 4, RULE_define_state_statement = 5, 
		RULE_constant_declaration = 6, RULE_value = 7, RULE_state = 8, RULE_initializing = 9, 
		RULE_initializing_item = 10, RULE_on_event_statement = 11, RULE_switch_state = 12, 
		RULE_execute = 13, RULE_execute2 = 14, RULE_execute3 = 15, RULE_matchings = 16, 
		RULE_matching = 17, RULE_action = 18, RULE_arguments = 19, RULE_argument = 20, 
		RULE_argumentValue = 21, RULE_rule_conditions = 22, RULE_event_declaration_statement = 23, 
		RULE_action_declaration_statement = 24, RULE_rule_declaration_statement = 25, 
		RULE_parameters = 26, RULE_parameter = 27, RULE_type = 28, RULE_key = 29, 
		RULE_compositekey = 30, RULE_comment = 31, RULE_number = 32, RULE_numeric = 33, 
		RULE_string = 34, RULE_delay = 35;
	public static final String[] ruleNames = {
		"script", "script_fragment", "script_full", "concurency", "define_referenciel_statement", 
		"define_state_statement", "constant_declaration", "value", "state", "initializing", 
		"initializing_item", "on_event_statement", "switch_state", "execute", 
		"execute2", "execute3", "matchings", "matching", "action", "arguments", 
		"argument", "argumentValue", "rule_conditions", "event_declaration_statement", 
		"action_declaration_statement", "rule_declaration_statement", "parameters", 
		"parameter", "type", "key", "compositekey", "comment", "number", "numeric", 
		"string", "delay"
	};

	private static final String[] _LITERAL_NAMES = {
		null, "'ACTION'", "'AFTER'", "'AND'", "'BEFORE'", "'CONST'", "'CONCURENCY'", 
		"'DESCRIPTION'", "'DAY'", "'DEFINE'", "'DECIMAL'", "'EVENT'", "'EXECUTE'", 
		"'ENTER'", "'='", "'EXIT'", "'EXPIRE'", "'FRAGMENT'", "'HOUR'", "'INCLUDE'", 
		"'INITIALIZE'", "'INTEGER'", "'KEY'", "'MATCHING'", "'MINUTE'", "'NAME'", 
		"'NOT'", "'ON'", "'OR'", "'PARAMETER'", "'RULE'", "'SAFE'", "'SWITCH'", 
		"'STATE'", "'STORE'", "'TEXT'", "'TIME'", "'THREAD'", "'VERSION'", "'WAITING'", 
		"'WITH'", "'WHEN'", "'WORKFLOW'", null, "'('", "')'", "';'", "':'", "','", 
		"'.'", "'@'"
	};
	private static final String[] _SYMBOLIC_NAMES = {
		null, "ACTION", "AFTER", "AND", "BEFORE", "CONST", "CONCURENCY", "DESCRIPTION", 
		"DAY", "DEFINE", "DECIMAL", "EVENT", "EXECUTE", "ENTER", "EQUAL", "EXIT", 
		"EXPIRE", "FRAGMENT", "HOUR", "INCLUDE", "INITIALIZE", "INTEGER", "KEY", 
		"MATCHING", "MINUTE", "NAME", "NOT", "ON", "OR", "PARAMETER", "RULE", 
		"SAFE", "SWITCH", "STATE", "STORE", "TEXT", "TIME", "THREAD", "VERSION", 
		"WAITING", "WITH", "WHEN", "WORKFLOW", "CHAR_STRING", "LEFT_PAREN", "RIGHT_PAREN", 
		"SEMICOLON", "COLON", "COMMA", "DOT", "AROBASE", "SPACES", "NUMBER", "SINGLE_LINE_COMMENT", 
		"MULTI_LINE_COMMENT", "REGULAR_ID"
	};
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "WorkflowParser.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public WorkflowParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}
	public static class ScriptContext extends ParserRuleContext {
		public Script_fragmentContext script_fragment() {
			return getRuleContext(Script_fragmentContext.class,0);
		}
		public Script_fullContext script_full() {
			return getRuleContext(Script_fullContext.class,0);
		}
		public TerminalNode EOF() { return getToken(WorkflowParser.EOF, 0); }
		public ScriptContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_script; }
	}

	public final ScriptContext script() throws RecognitionException {
		ScriptContext _localctx = new ScriptContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_script);
		try {
			setState(76);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case FRAGMENT:
				enterOuterAlt(_localctx, 1);
				{
				setState(72);
				script_fragment();
				}
				break;
			case INCLUDE:
			case NAME:
				enterOuterAlt(_localctx, 2);
				{
				setState(73);
				script_full();
				setState(74);
				match(EOF);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Script_fragmentContext extends ParserRuleContext {
		public TerminalNode FRAGMENT() { return getToken(WorkflowParser.FRAGMENT, 0); }
		public TerminalNode NAME() { return getToken(WorkflowParser.NAME, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode DESCRIPTION() { return getToken(WorkflowParser.DESCRIPTION, 0); }
		public CommentContext comment() {
			return getRuleContext(CommentContext.class,0);
		}
		public List<Define_referenciel_statementContext> define_referenciel_statement() {
			return getRuleContexts(Define_referenciel_statementContext.class);
		}
		public Define_referenciel_statementContext define_referenciel_statement(int i) {
			return getRuleContext(Define_referenciel_statementContext.class,i);
		}
		public List<TerminalNode> SEMICOLON() { return getTokens(WorkflowParser.SEMICOLON); }
		public TerminalNode SEMICOLON(int i) {
			return getToken(WorkflowParser.SEMICOLON, i);
		}
		public Script_fragmentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_script_fragment; }
	}

	public final Script_fragmentContext script_fragment() throws RecognitionException {
		Script_fragmentContext _localctx = new Script_fragmentContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_script_fragment);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(78);
			match(FRAGMENT);
			setState(79);
			match(NAME);
			setState(80);
			key();
			setState(83);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==DESCRIPTION) {
				{
				setState(81);
				match(DESCRIPTION);
				setState(82);
				comment();
				}
			}

			setState(88); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(85);
				define_referenciel_statement();
				setState(86);
				match(SEMICOLON);
				}
				}
				setState(90); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==DEFINE );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Script_fullContext extends ParserRuleContext {
		public NumberContext versionNumber;
		public TerminalNode NAME() { return getToken(WorkflowParser.NAME, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public List<TerminalNode> INCLUDE() { return getTokens(WorkflowParser.INCLUDE); }
		public TerminalNode INCLUDE(int i) {
			return getToken(WorkflowParser.INCLUDE, i);
		}
		public List<TerminalNode> CHAR_STRING() { return getTokens(WorkflowParser.CHAR_STRING); }
		public TerminalNode CHAR_STRING(int i) {
			return getToken(WorkflowParser.CHAR_STRING, i);
		}
		public TerminalNode VERSION() { return getToken(WorkflowParser.VERSION, 0); }
		public ConcurencyContext concurency() {
			return getRuleContext(ConcurencyContext.class,0);
		}
		public TerminalNode DESCRIPTION() { return getToken(WorkflowParser.DESCRIPTION, 0); }
		public CommentContext comment() {
			return getRuleContext(CommentContext.class,0);
		}
		public TerminalNode MATCHING() { return getToken(WorkflowParser.MATCHING, 0); }
		public MatchingsContext matchings() {
			return getRuleContext(MatchingsContext.class,0);
		}
		public List<Define_referenciel_statementContext> define_referenciel_statement() {
			return getRuleContexts(Define_referenciel_statementContext.class);
		}
		public Define_referenciel_statementContext define_referenciel_statement(int i) {
			return getRuleContext(Define_referenciel_statementContext.class,i);
		}
		public List<TerminalNode> SEMICOLON() { return getTokens(WorkflowParser.SEMICOLON); }
		public TerminalNode SEMICOLON(int i) {
			return getToken(WorkflowParser.SEMICOLON, i);
		}
		public InitializingContext initializing() {
			return getRuleContext(InitializingContext.class,0);
		}
		public List<Define_state_statementContext> define_state_statement() {
			return getRuleContexts(Define_state_statementContext.class);
		}
		public Define_state_statementContext define_state_statement(int i) {
			return getRuleContext(Define_state_statementContext.class,i);
		}
		public NumberContext number() {
			return getRuleContext(NumberContext.class,0);
		}
		public Script_fullContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_script_full; }
	}

	public final Script_fullContext script_full() throws RecognitionException {
		Script_fullContext _localctx = new Script_fullContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_script_full);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(96);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==INCLUDE) {
				{
				{
				setState(92);
				match(INCLUDE);
				setState(93);
				match(CHAR_STRING);
				}
				}
				setState(98);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(99);
			match(NAME);
			setState(100);
			key();
			setState(103);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==VERSION) {
				{
				setState(101);
				match(VERSION);
				setState(102);
				((Script_fullContext)_localctx).versionNumber = number();
				}
			}

			setState(106);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CONCURENCY) {
				{
				setState(105);
				concurency();
				}
			}

			setState(110);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==DESCRIPTION) {
				{
				setState(108);
				match(DESCRIPTION);
				setState(109);
				comment();
				}
			}

			setState(114);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==MATCHING) {
				{
				setState(112);
				match(MATCHING);
				setState(113);
				matchings();
				}
			}

			setState(119); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(116);
					define_referenciel_statement();
					setState(117);
					match(SEMICOLON);
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(121); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,8,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
			setState(124);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==INITIALIZE) {
				{
				setState(123);
				initializing();
				}
			}

			setState(131);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==DEFINE) {
				{
				{
				setState(126);
				define_state_statement();
				setState(127);
				match(SEMICOLON);
				}
				}
				setState(133);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ConcurencyContext extends ParserRuleContext {
		public NumberContext concurencyNumber;
		public TerminalNode CONCURENCY() { return getToken(WorkflowParser.CONCURENCY, 0); }
		public NumberContext number() {
			return getRuleContext(NumberContext.class,0);
		}
		public ConcurencyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_concurency; }
	}

	public final ConcurencyContext concurency() throws RecognitionException {
		ConcurencyContext _localctx = new ConcurencyContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_concurency);
		try {
			enterOuterAlt(_localctx, 1);
			{
			{
			setState(134);
			match(CONCURENCY);
			setState(135);
			((ConcurencyContext)_localctx).concurencyNumber = number();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Define_referenciel_statementContext extends ParserRuleContext {
		public TerminalNode DEFINE() { return getToken(WorkflowParser.DEFINE, 0); }
		public Event_declaration_statementContext event_declaration_statement() {
			return getRuleContext(Event_declaration_statementContext.class,0);
		}
		public Rule_declaration_statementContext rule_declaration_statement() {
			return getRuleContext(Rule_declaration_statementContext.class,0);
		}
		public Action_declaration_statementContext action_declaration_statement() {
			return getRuleContext(Action_declaration_statementContext.class,0);
		}
		public Constant_declarationContext constant_declaration() {
			return getRuleContext(Constant_declarationContext.class,0);
		}
		public Define_referenciel_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_define_referenciel_statement; }
	}

	public final Define_referenciel_statementContext define_referenciel_statement() throws RecognitionException {
		Define_referenciel_statementContext _localctx = new Define_referenciel_statementContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_define_referenciel_statement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(137);
			match(DEFINE);
			setState(142);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case EVENT:
				{
				setState(138);
				event_declaration_statement();
				}
				break;
			case RULE:
				{
				setState(139);
				rule_declaration_statement();
				}
				break;
			case ACTION:
				{
				setState(140);
				action_declaration_statement();
				}
				break;
			case CONST:
				{
				setState(141);
				constant_declaration();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Define_state_statementContext extends ParserRuleContext {
		public TerminalNode DEFINE() { return getToken(WorkflowParser.DEFINE, 0); }
		public StateContext state() {
			return getRuleContext(StateContext.class,0);
		}
		public Define_state_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_define_state_statement; }
	}

	public final Define_state_statementContext define_state_statement() throws RecognitionException {
		Define_state_statementContext _localctx = new Define_state_statementContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_define_state_statement);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(144);
			match(DEFINE);
			setState(145);
			state();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Constant_declarationContext extends ParserRuleContext {
		public TerminalNode CONST() { return getToken(WorkflowParser.CONST, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public ValueContext value() {
			return getRuleContext(ValueContext.class,0);
		}
		public CommentContext comment() {
			return getRuleContext(CommentContext.class,0);
		}
		public Constant_declarationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_constant_declaration; }
	}

	public final Constant_declarationContext constant_declaration() throws RecognitionException {
		Constant_declarationContext _localctx = new Constant_declarationContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_constant_declaration);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(147);
			match(CONST);
			setState(148);
			key();
			setState(149);
			value();
			setState(151);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CHAR_STRING) {
				{
				setState(150);
				comment();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ValueContext extends ParserRuleContext {
		public StringContext string() {
			return getRuleContext(StringContext.class,0);
		}
		public NumberContext number() {
			return getRuleContext(NumberContext.class,0);
		}
		public DelayContext delay() {
			return getRuleContext(DelayContext.class,0);
		}
		public TerminalNode REGULAR_ID() { return getToken(WorkflowParser.REGULAR_ID, 0); }
		public ValueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_value; }
	}

	public final ValueContext value() throws RecognitionException {
		ValueContext _localctx = new ValueContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_value);
		try {
			setState(157);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,13,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(153);
				string();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(154);
				number();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(155);
				delay();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(156);
				match(REGULAR_ID);
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StateContext extends ParserRuleContext {
		public TerminalNode STATE() { return getToken(WorkflowParser.STATE, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public CommentContext comment() {
			return getRuleContext(CommentContext.class,0);
		}
		public List<ExecuteContext> execute() {
			return getRuleContexts(ExecuteContext.class);
		}
		public ExecuteContext execute(int i) {
			return getRuleContext(ExecuteContext.class,i);
		}
		public List<On_event_statementContext> on_event_statement() {
			return getRuleContexts(On_event_statementContext.class);
		}
		public On_event_statementContext on_event_statement(int i) {
			return getRuleContext(On_event_statementContext.class,i);
		}
		public StateContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_state; }
	}

	public final StateContext state() throws RecognitionException {
		StateContext _localctx = new StateContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_state);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(159);
			match(STATE);
			setState(160);
			key();
			setState(162);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CHAR_STRING) {
				{
				setState(161);
				comment();
				}
			}

			setState(167);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,15,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					setState(164);
					execute();
					}
					} 
				}
				setState(169);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,15,_ctx);
			}
			setState(173);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==EXPIRE || _la==ON) {
				{
				{
				setState(170);
				on_event_statement();
				}
				}
				setState(175);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class InitializingContext extends ParserRuleContext {
		public TerminalNode INITIALIZE() { return getToken(WorkflowParser.INITIALIZE, 0); }
		public TerminalNode WORKFLOW() { return getToken(WorkflowParser.WORKFLOW, 0); }
		public List<Initializing_itemContext> initializing_item() {
			return getRuleContexts(Initializing_itemContext.class);
		}
		public Initializing_itemContext initializing_item(int i) {
			return getRuleContext(Initializing_itemContext.class,i);
		}
		public InitializingContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_initializing; }
	}

	public final InitializingContext initializing() throws RecognitionException {
		InitializingContext _localctx = new InitializingContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_initializing);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(176);
			match(INITIALIZE);
			setState(177);
			match(WORKFLOW);
			setState(179); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(178);
				initializing_item();
				}
				}
				setState(181); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==ON );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Initializing_itemContext extends ParserRuleContext {
		public TerminalNode ON() { return getToken(WorkflowParser.ON, 0); }
		public TerminalNode EVENT() { return getToken(WorkflowParser.EVENT, 0); }
		public List<KeyContext> key() {
			return getRuleContexts(KeyContext.class);
		}
		public KeyContext key(int i) {
			return getRuleContext(KeyContext.class,i);
		}
		public TerminalNode SWITCH() { return getToken(WorkflowParser.SWITCH, 0); }
		public TerminalNode WHEN() { return getToken(WorkflowParser.WHEN, 0); }
		public Rule_conditionsContext rule_conditions() {
			return getRuleContext(Rule_conditionsContext.class,0);
		}
		public Initializing_itemContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_initializing_item; }
	}

	public final Initializing_itemContext initializing_item() throws RecognitionException {
		Initializing_itemContext _localctx = new Initializing_itemContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_initializing_item);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(183);
			match(ON);
			setState(184);
			match(EVENT);
			setState(185);
			key();
			setState(188);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WHEN) {
				{
				setState(186);
				match(WHEN);
				setState(187);
				rule_conditions(0);
				}
			}

			setState(190);
			match(SWITCH);
			setState(191);
			key();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class On_event_statementContext extends ParserRuleContext {
		public TerminalNode ON() { return getToken(WorkflowParser.ON, 0); }
		public TerminalNode EVENT() { return getToken(WorkflowParser.EVENT, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode EXPIRE() { return getToken(WorkflowParser.EXPIRE, 0); }
		public TerminalNode AFTER() { return getToken(WorkflowParser.AFTER, 0); }
		public DelayContext delay() {
			return getRuleContext(DelayContext.class,0);
		}
		public List<Switch_stateContext> switch_state() {
			return getRuleContexts(Switch_stateContext.class);
		}
		public Switch_stateContext switch_state(int i) {
			return getRuleContext(Switch_stateContext.class,i);
		}
		public On_event_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_on_event_statement; }
	}

	public final On_event_statementContext on_event_statement() throws RecognitionException {
		On_event_statementContext _localctx = new On_event_statementContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_on_event_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(199);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case ON:
				{
				setState(193);
				match(ON);
				setState(194);
				match(EVENT);
				setState(195);
				key();
				}
				break;
			case EXPIRE:
				{
				setState(196);
				match(EXPIRE);
				setState(197);
				match(AFTER);
				setState(198);
				delay();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			setState(202); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(201);
				switch_state();
				}
				}
				setState(204); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( (((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << EXECUTE) | (1L << SWITCH) | (1L << STORE) | (1L << WAITING) | (1L << WHEN) | (1L << REGULAR_ID))) != 0) );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Switch_stateContext extends ParserRuleContext {
		public TerminalNode SWITCH() { return getToken(WorkflowParser.SWITCH, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode WHEN() { return getToken(WorkflowParser.WHEN, 0); }
		public Rule_conditionsContext rule_conditions() {
			return getRuleContext(Rule_conditionsContext.class,0);
		}
		public List<Execute2Context> execute2() {
			return getRuleContexts(Execute2Context.class);
		}
		public Execute2Context execute2(int i) {
			return getRuleContext(Execute2Context.class,i);
		}
		public Switch_stateContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_switch_state; }
	}

	public final Switch_stateContext switch_state() throws RecognitionException {
		Switch_stateContext _localctx = new Switch_stateContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_switch_state);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(208);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,21,_ctx) ) {
			case 1:
				{
				setState(206);
				match(WHEN);
				setState(207);
				rule_conditions(0);
				}
				break;
			}
			setState(223);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,24,_ctx) ) {
			case 1:
				{
				setState(213);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << EXECUTE) | (1L << STORE) | (1L << WAITING) | (1L << WHEN) | (1L << REGULAR_ID))) != 0)) {
					{
					{
					setState(210);
					execute2();
					}
					}
					setState(215);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(216);
				match(SWITCH);
				setState(217);
				key();
				}
				break;
			case 2:
				{
				setState(219); 
				_errHandler.sync(this);
				_alt = 1;
				do {
					switch (_alt) {
					case 1:
						{
						{
						setState(218);
						execute2();
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(221); 
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,23,_ctx);
				} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ExecuteContext extends ParserRuleContext {
		public TerminalNode ON() { return getToken(WorkflowParser.ON, 0); }
		public Execute2Context execute2() {
			return getRuleContext(Execute2Context.class,0);
		}
		public TerminalNode ENTER() { return getToken(WorkflowParser.ENTER, 0); }
		public TerminalNode STATE() { return getToken(WorkflowParser.STATE, 0); }
		public TerminalNode EXIT() { return getToken(WorkflowParser.EXIT, 0); }
		public TerminalNode AND() { return getToken(WorkflowParser.AND, 0); }
		public ExecuteContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_execute; }
	}

	public final ExecuteContext execute() throws RecognitionException {
		ExecuteContext _localctx = new ExecuteContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_execute);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(225);
			match(ON);
			setState(234);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,25,_ctx) ) {
			case 1:
				{
				setState(226);
				match(ENTER);
				setState(227);
				match(STATE);
				}
				break;
			case 2:
				{
				setState(228);
				match(EXIT);
				setState(229);
				match(STATE);
				}
				break;
			case 3:
				{
				setState(230);
				match(ENTER);
				setState(231);
				match(AND);
				setState(232);
				match(EXIT);
				setState(233);
				match(STATE);
				}
				break;
			}
			setState(236);
			execute2();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Execute2Context extends ParserRuleContext {
		public TerminalNode WHEN() { return getToken(WorkflowParser.WHEN, 0); }
		public Rule_conditionsContext rule_conditions() {
			return getRuleContext(Rule_conditionsContext.class,0);
		}
		public TerminalNode WAITING() { return getToken(WorkflowParser.WAITING, 0); }
		public DelayContext delay() {
			return getRuleContext(DelayContext.class,0);
		}
		public TerminalNode BEFORE() { return getToken(WorkflowParser.BEFORE, 0); }
		public List<Execute3Context> execute3() {
			return getRuleContexts(Execute3Context.class);
		}
		public Execute3Context execute3(int i) {
			return getRuleContext(Execute3Context.class,i);
		}
		public Execute2Context(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_execute2; }
	}

	public final Execute2Context execute2() throws RecognitionException {
		Execute2Context _localctx = new Execute2Context(_ctx, getState());
		enterRule(_localctx, 28, RULE_execute2);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(240);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WHEN) {
				{
				setState(238);
				match(WHEN);
				setState(239);
				rule_conditions(0);
				}
			}

			setState(246);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==WAITING) {
				{
				setState(242);
				match(WAITING);
				setState(243);
				delay();
				setState(244);
				match(BEFORE);
				}
			}

			setState(249); 
			_errHandler.sync(this);
			_alt = 1;
			do {
				switch (_alt) {
				case 1:
					{
					{
					setState(248);
					execute3();
					}
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(251); 
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,28,_ctx);
			} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Execute3Context extends ParserRuleContext {
		public TerminalNode EXECUTE() { return getToken(WorkflowParser.EXECUTE, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public List<ActionContext> action() {
			return getRuleContexts(ActionContext.class);
		}
		public ActionContext action(int i) {
			return getRuleContext(ActionContext.class,i);
		}
		public TerminalNode STORE() { return getToken(WorkflowParser.STORE, 0); }
		public List<MatchingsContext> matchings() {
			return getRuleContexts(MatchingsContext.class);
		}
		public MatchingsContext matchings(int i) {
			return getRuleContext(MatchingsContext.class,i);
		}
		public Execute3Context(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_execute3; }
	}

	public final Execute3Context execute3() throws RecognitionException {
		Execute3Context _localctx = new Execute3Context(_ctx, getState());
		enterRule(_localctx, 30, RULE_execute3);
		int _la;
		try {
			int _alt;
			setState(268);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case EXECUTE:
			case REGULAR_ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(255);
				_errHandler.sync(this);
				switch (_input.LA(1)) {
				case EXECUTE:
					{
					setState(253);
					match(EXECUTE);
					}
					break;
				case REGULAR_ID:
					{
					setState(254);
					key();
					}
					break;
				default:
					throw new NoViableAltException(this);
				}
				setState(258); 
				_errHandler.sync(this);
				_alt = 1;
				do {
					switch (_alt) {
					case 1:
						{
						{
						setState(257);
						action();
						}
						}
						break;
					default:
						throw new NoViableAltException(this);
					}
					setState(260); 
					_errHandler.sync(this);
					_alt = getInterpreter().adaptivePredict(_input,30,_ctx);
				} while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER );
				}
				break;
			case STORE:
				enterOuterAlt(_localctx, 2);
				{
				setState(262);
				match(STORE);
				setState(264); 
				_errHandler.sync(this);
				_la = _input.LA(1);
				do {
					{
					{
					setState(263);
					matchings();
					}
					}
					setState(266); 
					_errHandler.sync(this);
					_la = _input.LA(1);
				} while ( _la==LEFT_PAREN );
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class MatchingsContext extends ParserRuleContext {
		public TerminalNode LEFT_PAREN() { return getToken(WorkflowParser.LEFT_PAREN, 0); }
		public TerminalNode RIGHT_PAREN() { return getToken(WorkflowParser.RIGHT_PAREN, 0); }
		public List<MatchingContext> matching() {
			return getRuleContexts(MatchingContext.class);
		}
		public MatchingContext matching(int i) {
			return getRuleContext(MatchingContext.class,i);
		}
		public MatchingsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_matchings; }
	}

	public final MatchingsContext matchings() throws RecognitionException {
		MatchingsContext _localctx = new MatchingsContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_matchings);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(270);
			match(LEFT_PAREN);
			setState(272); 
			_errHandler.sync(this);
			_la = _input.LA(1);
			do {
				{
				{
				setState(271);
				matching();
				}
				}
				setState(274); 
				_errHandler.sync(this);
				_la = _input.LA(1);
			} while ( _la==REGULAR_ID );
			setState(276);
			match(RIGHT_PAREN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class MatchingContext extends ParserRuleContext {
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode EQUAL() { return getToken(WorkflowParser.EQUAL, 0); }
		public StringContext string() {
			return getRuleContext(StringContext.class,0);
		}
		public MatchingContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_matching; }
	}

	public final MatchingContext matching() throws RecognitionException {
		MatchingContext _localctx = new MatchingContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_matching);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(278);
			key();
			setState(279);
			match(EQUAL);
			setState(280);
			string();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ActionContext extends ParserRuleContext {
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode LEFT_PAREN() { return getToken(WorkflowParser.LEFT_PAREN, 0); }
		public TerminalNode RIGHT_PAREN() { return getToken(WorkflowParser.RIGHT_PAREN, 0); }
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public ActionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_action; }
	}

	public final ActionContext action() throws RecognitionException {
		ActionContext _localctx = new ActionContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_action);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(282);
			key();
			setState(283);
			match(LEFT_PAREN);
			setState(285);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==REGULAR_ID) {
				{
				setState(284);
				arguments();
				}
			}

			setState(287);
			match(RIGHT_PAREN);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArgumentsContext extends ParserRuleContext {
		public List<ArgumentContext> argument() {
			return getRuleContexts(ArgumentContext.class);
		}
		public ArgumentContext argument(int i) {
			return getRuleContext(ArgumentContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(WorkflowParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(WorkflowParser.COMMA, i);
		}
		public ArgumentsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arguments; }
	}

	public final ArgumentsContext arguments() throws RecognitionException {
		ArgumentsContext _localctx = new ArgumentsContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_arguments);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(289);
			argument();
			setState(294);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(290);
				match(COMMA);
				setState(291);
				argument();
				}
				}
				setState(296);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArgumentContext extends ParserRuleContext {
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode EQUAL() { return getToken(WorkflowParser.EQUAL, 0); }
		public ArgumentValueContext argumentValue() {
			return getRuleContext(ArgumentValueContext.class,0);
		}
		public ArgumentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_argument; }
	}

	public final ArgumentContext argument() throws RecognitionException {
		ArgumentContext _localctx = new ArgumentContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_argument);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(297);
			key();
			setState(298);
			match(EQUAL);
			setState(299);
			argumentValue();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ArgumentValueContext extends ParserRuleContext {
		public StringContext string() {
			return getRuleContext(StringContext.class,0);
		}
		public CompositekeyContext compositekey() {
			return getRuleContext(CompositekeyContext.class,0);
		}
		public ArgumentValueContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_argumentValue; }
	}

	public final ArgumentValueContext argumentValue() throws RecognitionException {
		ArgumentValueContext _localctx = new ArgumentValueContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_argumentValue);
		try {
			setState(303);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case CHAR_STRING:
				enterOuterAlt(_localctx, 1);
				{
				setState(301);
				string();
				}
				break;
			case AROBASE:
			case REGULAR_ID:
				enterOuterAlt(_localctx, 2);
				{
				setState(302);
				compositekey();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Rule_conditionsContext extends ParserRuleContext {
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode LEFT_PAREN() { return getToken(WorkflowParser.LEFT_PAREN, 0); }
		public TerminalNode RIGHT_PAREN() { return getToken(WorkflowParser.RIGHT_PAREN, 0); }
		public ArgumentsContext arguments() {
			return getRuleContext(ArgumentsContext.class,0);
		}
		public TerminalNode NOT() { return getToken(WorkflowParser.NOT, 0); }
		public List<Rule_conditionsContext> rule_conditions() {
			return getRuleContexts(Rule_conditionsContext.class);
		}
		public Rule_conditionsContext rule_conditions(int i) {
			return getRuleContext(Rule_conditionsContext.class,i);
		}
		public TerminalNode AND() { return getToken(WorkflowParser.AND, 0); }
		public TerminalNode OR() { return getToken(WorkflowParser.OR, 0); }
		public Rule_conditionsContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_rule_conditions; }
	}

	public final Rule_conditionsContext rule_conditions() throws RecognitionException {
		return rule_conditions(0);
	}

	private Rule_conditionsContext rule_conditions(int _p) throws RecognitionException {
		ParserRuleContext _parentctx = _ctx;
		int _parentState = getState();
		Rule_conditionsContext _localctx = new Rule_conditionsContext(_ctx, _parentState);
		Rule_conditionsContext _prevctx = _localctx;
		int _startState = 44;
		enterRecursionRule(_localctx, 44, RULE_rule_conditions, _p);
		int _la;
		try {
			int _alt;
			enterOuterAlt(_localctx, 1);
			{
			setState(319);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case REGULAR_ID:
				{
				setState(306);
				key();
				setState(307);
				match(LEFT_PAREN);
				setState(309);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==REGULAR_ID) {
					{
					setState(308);
					arguments();
					}
				}

				setState(311);
				match(RIGHT_PAREN);
				}
				break;
			case NOT:
				{
				setState(313);
				match(NOT);
				setState(314);
				rule_conditions(4);
				}
				break;
			case LEFT_PAREN:
				{
				setState(315);
				match(LEFT_PAREN);
				setState(316);
				rule_conditions(0);
				setState(317);
				match(RIGHT_PAREN);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			_ctx.stop = _input.LT(-1);
			setState(329);
			_errHandler.sync(this);
			_alt = getInterpreter().adaptivePredict(_input,40,_ctx);
			while ( _alt!=2 && _alt!=org.antlr.v4.runtime.atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( _parseListeners!=null ) triggerExitRuleEvent();
					_prevctx = _localctx;
					{
					setState(327);
					_errHandler.sync(this);
					switch ( getInterpreter().adaptivePredict(_input,39,_ctx) ) {
					case 1:
						{
						_localctx = new Rule_conditionsContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_rule_conditions);
						setState(321);
						if (!(precpred(_ctx, 3))) throw new FailedPredicateException(this, "precpred(_ctx, 3)");
						setState(322);
						match(AND);
						setState(323);
						rule_conditions(4);
						}
						break;
					case 2:
						{
						_localctx = new Rule_conditionsContext(_parentctx, _parentState);
						pushNewRecursionContext(_localctx, _startState, RULE_rule_conditions);
						setState(324);
						if (!(precpred(_ctx, 2))) throw new FailedPredicateException(this, "precpred(_ctx, 2)");
						setState(325);
						match(OR);
						setState(326);
						rule_conditions(3);
						}
						break;
					}
					} 
				}
				setState(331);
				_errHandler.sync(this);
				_alt = getInterpreter().adaptivePredict(_input,40,_ctx);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			unrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public static class Event_declaration_statementContext extends ParserRuleContext {
		public TerminalNode EVENT() { return getToken(WorkflowParser.EVENT, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public CommentContext comment() {
			return getRuleContext(CommentContext.class,0);
		}
		public Event_declaration_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_event_declaration_statement; }
	}

	public final Event_declaration_statementContext event_declaration_statement() throws RecognitionException {
		Event_declaration_statementContext _localctx = new Event_declaration_statementContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_event_declaration_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(332);
			match(EVENT);
			setState(333);
			key();
			setState(335);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CHAR_STRING) {
				{
				setState(334);
				comment();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Action_declaration_statementContext extends ParserRuleContext {
		public TerminalNode ACTION() { return getToken(WorkflowParser.ACTION, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode LEFT_PAREN() { return getToken(WorkflowParser.LEFT_PAREN, 0); }
		public TerminalNode RIGHT_PAREN() { return getToken(WorkflowParser.RIGHT_PAREN, 0); }
		public ParametersContext parameters() {
			return getRuleContext(ParametersContext.class,0);
		}
		public CommentContext comment() {
			return getRuleContext(CommentContext.class,0);
		}
		public Action_declaration_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_action_declaration_statement; }
	}

	public final Action_declaration_statementContext action_declaration_statement() throws RecognitionException {
		Action_declaration_statementContext _localctx = new Action_declaration_statementContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_action_declaration_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(337);
			match(ACTION);
			setState(338);
			key();
			setState(339);
			match(LEFT_PAREN);
			setState(341);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DECIMAL) | (1L << INTEGER) | (1L << TEXT))) != 0)) {
				{
				setState(340);
				parameters();
				}
			}

			setState(343);
			match(RIGHT_PAREN);
			setState(345);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CHAR_STRING) {
				{
				setState(344);
				comment();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class Rule_declaration_statementContext extends ParserRuleContext {
		public TerminalNode RULE() { return getToken(WorkflowParser.RULE, 0); }
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public TerminalNode LEFT_PAREN() { return getToken(WorkflowParser.LEFT_PAREN, 0); }
		public TerminalNode RIGHT_PAREN() { return getToken(WorkflowParser.RIGHT_PAREN, 0); }
		public ParametersContext parameters() {
			return getRuleContext(ParametersContext.class,0);
		}
		public CommentContext comment() {
			return getRuleContext(CommentContext.class,0);
		}
		public Rule_declaration_statementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_rule_declaration_statement; }
	}

	public final Rule_declaration_statementContext rule_declaration_statement() throws RecognitionException {
		Rule_declaration_statementContext _localctx = new Rule_declaration_statementContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_rule_declaration_statement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(347);
			match(RULE);
			setState(348);
			key();
			setState(349);
			match(LEFT_PAREN);
			setState(351);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DECIMAL) | (1L << INTEGER) | (1L << TEXT))) != 0)) {
				{
				setState(350);
				parameters();
				}
			}

			setState(353);
			match(RIGHT_PAREN);
			setState(355);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==CHAR_STRING) {
				{
				setState(354);
				comment();
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ParametersContext extends ParserRuleContext {
		public List<ParameterContext> parameter() {
			return getRuleContexts(ParameterContext.class);
		}
		public ParameterContext parameter(int i) {
			return getRuleContext(ParameterContext.class,i);
		}
		public List<TerminalNode> COMMA() { return getTokens(WorkflowParser.COMMA); }
		public TerminalNode COMMA(int i) {
			return getToken(WorkflowParser.COMMA, i);
		}
		public ParametersContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parameters; }
	}

	public final ParametersContext parameters() throws RecognitionException {
		ParametersContext _localctx = new ParametersContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_parameters);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(357);
			parameter();
			setState(362);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==COMMA) {
				{
				{
				setState(358);
				match(COMMA);
				setState(359);
				parameter();
				}
				}
				setState(364);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class ParameterContext extends ParserRuleContext {
		public TypeContext type() {
			return getRuleContext(TypeContext.class,0);
		}
		public KeyContext key() {
			return getRuleContext(KeyContext.class,0);
		}
		public ParameterContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_parameter; }
	}

	public final ParameterContext parameter() throws RecognitionException {
		ParameterContext _localctx = new ParameterContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_parameter);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(365);
			type();
			setState(366);
			key();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class TypeContext extends ParserRuleContext {
		public TerminalNode TEXT() { return getToken(WorkflowParser.TEXT, 0); }
		public TerminalNode INTEGER() { return getToken(WorkflowParser.INTEGER, 0); }
		public TerminalNode DECIMAL() { return getToken(WorkflowParser.DECIMAL, 0); }
		public TypeContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_type; }
	}

	public final TypeContext type() throws RecognitionException {
		TypeContext _localctx = new TypeContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_type);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(368);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DECIMAL) | (1L << INTEGER) | (1L << TEXT))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class KeyContext extends ParserRuleContext {
		public TerminalNode REGULAR_ID() { return getToken(WorkflowParser.REGULAR_ID, 0); }
		public KeyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_key; }
	}

	public final KeyContext key() throws RecognitionException {
		KeyContext _localctx = new KeyContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_key);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(370);
			match(REGULAR_ID);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class CompositekeyContext extends ParserRuleContext {
		public List<KeyContext> key() {
			return getRuleContexts(KeyContext.class);
		}
		public KeyContext key(int i) {
			return getRuleContext(KeyContext.class,i);
		}
		public TerminalNode AROBASE() { return getToken(WorkflowParser.AROBASE, 0); }
		public List<TerminalNode> DOT() { return getTokens(WorkflowParser.DOT); }
		public TerminalNode DOT(int i) {
			return getToken(WorkflowParser.DOT, i);
		}
		public CompositekeyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_compositekey; }
	}

	public final CompositekeyContext compositekey() throws RecognitionException {
		CompositekeyContext _localctx = new CompositekeyContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_compositekey);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(373);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==AROBASE) {
				{
				setState(372);
				match(AROBASE);
				}
			}

			setState(375);
			key();
			setState(380);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==DOT) {
				{
				{
				setState(376);
				match(DOT);
				setState(377);
				key();
				}
				}
				setState(382);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class CommentContext extends ParserRuleContext {
		public TerminalNode CHAR_STRING() { return getToken(WorkflowParser.CHAR_STRING, 0); }
		public CommentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_comment; }
	}

	public final CommentContext comment() throws RecognitionException {
		CommentContext _localctx = new CommentContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_comment);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(383);
			match(CHAR_STRING);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class NumberContext extends ParserRuleContext {
		public TerminalNode NUMBER() { return getToken(WorkflowParser.NUMBER, 0); }
		public NumberContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_number; }
	}

	public final NumberContext number() throws RecognitionException {
		NumberContext _localctx = new NumberContext(_ctx, getState());
		enterRule(_localctx, 64, RULE_number);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(385);
			match(NUMBER);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class NumericContext extends ParserRuleContext {
		public List<NumberContext> number() {
			return getRuleContexts(NumberContext.class);
		}
		public NumberContext number(int i) {
			return getRuleContext(NumberContext.class,i);
		}
		public TerminalNode DOT() { return getToken(WorkflowParser.DOT, 0); }
		public NumericContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_numeric; }
	}

	public final NumericContext numeric() throws RecognitionException {
		NumericContext _localctx = new NumericContext(_ctx, getState());
		enterRule(_localctx, 66, RULE_numeric);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(387);
			number();
			setState(388);
			match(DOT);
			setState(389);
			number();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class StringContext extends ParserRuleContext {
		public TerminalNode CHAR_STRING() { return getToken(WorkflowParser.CHAR_STRING, 0); }
		public StringContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_string; }
	}

	public final StringContext string() throws RecognitionException {
		StringContext _localctx = new StringContext(_ctx, getState());
		enterRule(_localctx, 68, RULE_string);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(391);
			match(CHAR_STRING);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static class DelayContext extends ParserRuleContext {
		public NumberContext number() {
			return getRuleContext(NumberContext.class,0);
		}
		public TerminalNode MINUTE() { return getToken(WorkflowParser.MINUTE, 0); }
		public TerminalNode HOUR() { return getToken(WorkflowParser.HOUR, 0); }
		public TerminalNode DAY() { return getToken(WorkflowParser.DAY, 0); }
		public DelayContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_delay; }
	}

	public final DelayContext delay() throws RecognitionException {
		DelayContext _localctx = new DelayContext(_ctx, getState());
		enterRule(_localctx, 70, RULE_delay);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(393);
			number();
			setState(394);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & ((1L << DAY) | (1L << HOUR) | (1L << MINUTE))) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public boolean sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 22:
			return rule_conditions_sempred((Rule_conditionsContext)_localctx, predIndex);
		}
		return true;
	}
	private boolean rule_conditions_sempred(Rule_conditionsContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0:
			return precpred(_ctx, 3);
		case 1:
			return precpred(_ctx, 2);
		}
		return true;
	}

	public static final String _serializedATN =
		"\3\u608b\ua72a\u8133\ub9ed\u417c\u3be7\u7786\u5964\39\u018f\4\2\t\2\4"+
		"\3\t\3\4\4\t\4\4\5\t\5\4\6\t\6\4\7\t\7\4\b\t\b\4\t\t\t\4\n\t\n\4\13\t"+
		"\13\4\f\t\f\4\r\t\r\4\16\t\16\4\17\t\17\4\20\t\20\4\21\t\21\4\22\t\22"+
		"\4\23\t\23\4\24\t\24\4\25\t\25\4\26\t\26\4\27\t\27\4\30\t\30\4\31\t\31"+
		"\4\32\t\32\4\33\t\33\4\34\t\34\4\35\t\35\4\36\t\36\4\37\t\37\4 \t \4!"+
		"\t!\4\"\t\"\4#\t#\4$\t$\4%\t%\3\2\3\2\3\2\3\2\5\2O\n\2\3\3\3\3\3\3\3\3"+
		"\3\3\5\3V\n\3\3\3\3\3\3\3\6\3[\n\3\r\3\16\3\\\3\4\3\4\7\4a\n\4\f\4\16"+
		"\4d\13\4\3\4\3\4\3\4\3\4\5\4j\n\4\3\4\5\4m\n\4\3\4\3\4\5\4q\n\4\3\4\3"+
		"\4\5\4u\n\4\3\4\3\4\3\4\6\4z\n\4\r\4\16\4{\3\4\5\4\177\n\4\3\4\3\4\3\4"+
		"\7\4\u0084\n\4\f\4\16\4\u0087\13\4\3\5\3\5\3\5\3\6\3\6\3\6\3\6\3\6\5\6"+
		"\u0091\n\6\3\7\3\7\3\7\3\b\3\b\3\b\3\b\5\b\u009a\n\b\3\t\3\t\3\t\3\t\5"+
		"\t\u00a0\n\t\3\n\3\n\3\n\5\n\u00a5\n\n\3\n\7\n\u00a8\n\n\f\n\16\n\u00ab"+
		"\13\n\3\n\7\n\u00ae\n\n\f\n\16\n\u00b1\13\n\3\13\3\13\3\13\6\13\u00b6"+
		"\n\13\r\13\16\13\u00b7\3\f\3\f\3\f\3\f\3\f\5\f\u00bf\n\f\3\f\3\f\3\f\3"+
		"\r\3\r\3\r\3\r\3\r\3\r\5\r\u00ca\n\r\3\r\6\r\u00cd\n\r\r\r\16\r\u00ce"+
		"\3\16\3\16\5\16\u00d3\n\16\3\16\7\16\u00d6\n\16\f\16\16\16\u00d9\13\16"+
		"\3\16\3\16\3\16\6\16\u00de\n\16\r\16\16\16\u00df\5\16\u00e2\n\16\3\17"+
		"\3\17\3\17\3\17\3\17\3\17\3\17\3\17\3\17\5\17\u00ed\n\17\3\17\3\17\3\20"+
		"\3\20\5\20\u00f3\n\20\3\20\3\20\3\20\3\20\5\20\u00f9\n\20\3\20\6\20\u00fc"+
		"\n\20\r\20\16\20\u00fd\3\21\3\21\5\21\u0102\n\21\3\21\6\21\u0105\n\21"+
		"\r\21\16\21\u0106\3\21\3\21\6\21\u010b\n\21\r\21\16\21\u010c\5\21\u010f"+
		"\n\21\3\22\3\22\6\22\u0113\n\22\r\22\16\22\u0114\3\22\3\22\3\23\3\23\3"+
		"\23\3\23\3\24\3\24\3\24\5\24\u0120\n\24\3\24\3\24\3\25\3\25\3\25\7\25"+
		"\u0127\n\25\f\25\16\25\u012a\13\25\3\26\3\26\3\26\3\26\3\27\3\27\5\27"+
		"\u0132\n\27\3\30\3\30\3\30\3\30\5\30\u0138\n\30\3\30\3\30\3\30\3\30\3"+
		"\30\3\30\3\30\3\30\5\30\u0142\n\30\3\30\3\30\3\30\3\30\3\30\3\30\7\30"+
		"\u014a\n\30\f\30\16\30\u014d\13\30\3\31\3\31\3\31\5\31\u0152\n\31\3\32"+
		"\3\32\3\32\3\32\5\32\u0158\n\32\3\32\3\32\5\32\u015c\n\32\3\33\3\33\3"+
		"\33\3\33\5\33\u0162\n\33\3\33\3\33\5\33\u0166\n\33\3\34\3\34\3\34\7\34"+
		"\u016b\n\34\f\34\16\34\u016e\13\34\3\35\3\35\3\35\3\36\3\36\3\37\3\37"+
		"\3 \5 \u0178\n \3 \3 \3 \7 \u017d\n \f \16 \u0180\13 \3!\3!\3\"\3\"\3"+
		"#\3#\3#\3#\3$\3$\3%\3%\3%\3%\2\3.&\2\4\6\b\n\f\16\20\22\24\26\30\32\34"+
		"\36 \"$&(*,.\60\62\64\668:<>@BDFH\2\4\5\2\f\f\27\27%%\5\2\n\n\24\24\32"+
		"\32\2\u01a1\2N\3\2\2\2\4P\3\2\2\2\6b\3\2\2\2\b\u0088\3\2\2\2\n\u008b\3"+
		"\2\2\2\f\u0092\3\2\2\2\16\u0095\3\2\2\2\20\u009f\3\2\2\2\22\u00a1\3\2"+
		"\2\2\24\u00b2\3\2\2\2\26\u00b9\3\2\2\2\30\u00c9\3\2\2\2\32\u00d2\3\2\2"+
		"\2\34\u00e3\3\2\2\2\36\u00f2\3\2\2\2 \u010e\3\2\2\2\"\u0110\3\2\2\2$\u0118"+
		"\3\2\2\2&\u011c\3\2\2\2(\u0123\3\2\2\2*\u012b\3\2\2\2,\u0131\3\2\2\2."+
		"\u0141\3\2\2\2\60\u014e\3\2\2\2\62\u0153\3\2\2\2\64\u015d\3\2\2\2\66\u0167"+
		"\3\2\2\28\u016f\3\2\2\2:\u0172\3\2\2\2<\u0174\3\2\2\2>\u0177\3\2\2\2@"+
		"\u0181\3\2\2\2B\u0183\3\2\2\2D\u0185\3\2\2\2F\u0189\3\2\2\2H\u018b\3\2"+
		"\2\2JO\5\4\3\2KL\5\6\4\2LM\7\2\2\3MO\3\2\2\2NJ\3\2\2\2NK\3\2\2\2O\3\3"+
		"\2\2\2PQ\7\23\2\2QR\7\33\2\2RU\5<\37\2ST\7\t\2\2TV\5@!\2US\3\2\2\2UV\3"+
		"\2\2\2VZ\3\2\2\2WX\5\n\6\2XY\7\60\2\2Y[\3\2\2\2ZW\3\2\2\2[\\\3\2\2\2\\"+
		"Z\3\2\2\2\\]\3\2\2\2]\5\3\2\2\2^_\7\25\2\2_a\7-\2\2`^\3\2\2\2ad\3\2\2"+
		"\2b`\3\2\2\2bc\3\2\2\2ce\3\2\2\2db\3\2\2\2ef\7\33\2\2fi\5<\37\2gh\7(\2"+
		"\2hj\5B\"\2ig\3\2\2\2ij\3\2\2\2jl\3\2\2\2km\5\b\5\2lk\3\2\2\2lm\3\2\2"+
		"\2mp\3\2\2\2no\7\t\2\2oq\5@!\2pn\3\2\2\2pq\3\2\2\2qt\3\2\2\2rs\7\31\2"+
		"\2su\5\"\22\2tr\3\2\2\2tu\3\2\2\2uy\3\2\2\2vw\5\n\6\2wx\7\60\2\2xz\3\2"+
		"\2\2yv\3\2\2\2z{\3\2\2\2{y\3\2\2\2{|\3\2\2\2|~\3\2\2\2}\177\5\24\13\2"+
		"~}\3\2\2\2~\177\3\2\2\2\177\u0085\3\2\2\2\u0080\u0081\5\f\7\2\u0081\u0082"+
		"\7\60\2\2\u0082\u0084\3\2\2\2\u0083\u0080\3\2\2\2\u0084\u0087\3\2\2\2"+
		"\u0085\u0083\3\2\2\2\u0085\u0086\3\2\2\2\u0086\7\3\2\2\2\u0087\u0085\3"+
		"\2\2\2\u0088\u0089\7\b\2\2\u0089\u008a\5B\"\2\u008a\t\3\2\2\2\u008b\u0090"+
		"\7\13\2\2\u008c\u0091\5\60\31\2\u008d\u0091\5\64\33\2\u008e\u0091\5\62"+
		"\32\2\u008f\u0091\5\16\b\2\u0090\u008c\3\2\2\2\u0090\u008d\3\2\2\2\u0090"+
		"\u008e\3\2\2\2\u0090\u008f\3\2\2\2\u0091\13\3\2\2\2\u0092\u0093\7\13\2"+
		"\2\u0093\u0094\5\22\n\2\u0094\r\3\2\2\2\u0095\u0096\7\7\2\2\u0096\u0097"+
		"\5<\37\2\u0097\u0099\5\20\t\2\u0098\u009a\5@!\2\u0099\u0098\3\2\2\2\u0099"+
		"\u009a\3\2\2\2\u009a\17\3\2\2\2\u009b\u00a0\5F$\2\u009c\u00a0\5B\"\2\u009d"+
		"\u00a0\5H%\2\u009e\u00a0\79\2\2\u009f\u009b\3\2\2\2\u009f\u009c\3\2\2"+
		"\2\u009f\u009d\3\2\2\2\u009f\u009e\3\2\2\2\u00a0\21\3\2\2\2\u00a1\u00a2"+
		"\7#\2\2\u00a2\u00a4\5<\37\2\u00a3\u00a5\5@!\2\u00a4\u00a3\3\2\2\2\u00a4"+
		"\u00a5\3\2\2\2\u00a5\u00a9\3\2\2\2\u00a6\u00a8\5\34\17\2\u00a7\u00a6\3"+
		"\2\2\2\u00a8\u00ab\3\2\2\2\u00a9\u00a7\3\2\2\2\u00a9\u00aa\3\2\2\2\u00aa"+
		"\u00af\3\2\2\2\u00ab\u00a9\3\2\2\2\u00ac\u00ae\5\30\r\2\u00ad\u00ac\3"+
		"\2\2\2\u00ae\u00b1\3\2\2\2\u00af\u00ad\3\2\2\2\u00af\u00b0\3\2\2\2\u00b0"+
		"\23\3\2\2\2\u00b1\u00af\3\2\2\2\u00b2\u00b3\7\26\2\2\u00b3\u00b5\7,\2"+
		"\2\u00b4\u00b6\5\26\f\2\u00b5\u00b4\3\2\2\2\u00b6\u00b7\3\2\2\2\u00b7"+
		"\u00b5\3\2\2\2\u00b7\u00b8\3\2\2\2\u00b8\25\3\2\2\2\u00b9\u00ba\7\35\2"+
		"\2\u00ba\u00bb\7\r\2\2\u00bb\u00be\5<\37\2\u00bc\u00bd\7+\2\2\u00bd\u00bf"+
		"\5.\30\2\u00be\u00bc\3\2\2\2\u00be\u00bf\3\2\2\2\u00bf\u00c0\3\2\2\2\u00c0"+
		"\u00c1\7\"\2\2\u00c1\u00c2\5<\37\2\u00c2\27\3\2\2\2\u00c3\u00c4\7\35\2"+
		"\2\u00c4\u00c5\7\r\2\2\u00c5\u00ca\5<\37\2\u00c6\u00c7\7\22\2\2\u00c7"+
		"\u00c8\7\4\2\2\u00c8\u00ca\5H%\2\u00c9\u00c3\3\2\2\2\u00c9\u00c6\3\2\2"+
		"\2\u00ca\u00cc\3\2\2\2\u00cb\u00cd\5\32\16\2\u00cc\u00cb\3\2\2\2\u00cd"+
		"\u00ce\3\2\2\2\u00ce\u00cc\3\2\2\2\u00ce\u00cf\3\2\2\2\u00cf\31\3\2\2"+
		"\2\u00d0\u00d1\7+\2\2\u00d1\u00d3\5.\30\2\u00d2\u00d0\3\2\2\2\u00d2\u00d3"+
		"\3\2\2\2\u00d3\u00e1\3\2\2\2\u00d4\u00d6\5\36\20\2\u00d5\u00d4\3\2\2\2"+
		"\u00d6\u00d9\3\2\2\2\u00d7\u00d5\3\2\2\2\u00d7\u00d8\3\2\2\2\u00d8\u00da"+
		"\3\2\2\2\u00d9\u00d7\3\2\2\2\u00da\u00db\7\"\2\2\u00db\u00e2\5<\37\2\u00dc"+
		"\u00de\5\36\20\2\u00dd\u00dc\3\2\2\2\u00de\u00df\3\2\2\2\u00df\u00dd\3"+
		"\2\2\2\u00df\u00e0\3\2\2\2\u00e0\u00e2\3\2\2\2\u00e1\u00d7\3\2\2\2\u00e1"+
		"\u00dd\3\2\2\2\u00e2\33\3\2\2\2\u00e3\u00ec\7\35\2\2\u00e4\u00e5\7\17"+
		"\2\2\u00e5\u00ed\7#\2\2\u00e6\u00e7\7\21\2\2\u00e7\u00ed\7#\2\2\u00e8"+
		"\u00e9\7\17\2\2\u00e9\u00ea\7\5\2\2\u00ea\u00eb\7\21\2\2\u00eb\u00ed\7"+
		"#\2\2\u00ec\u00e4\3\2\2\2\u00ec\u00e6\3\2\2\2\u00ec\u00e8\3\2\2\2\u00ed"+
		"\u00ee\3\2\2\2\u00ee\u00ef\5\36\20\2\u00ef\35\3\2\2\2\u00f0\u00f1\7+\2"+
		"\2\u00f1\u00f3\5.\30\2\u00f2\u00f0\3\2\2\2\u00f2\u00f3\3\2\2\2\u00f3\u00f8"+
		"\3\2\2\2\u00f4\u00f5\7)\2\2\u00f5\u00f6\5H%\2\u00f6\u00f7\7\6\2\2\u00f7"+
		"\u00f9\3\2\2\2\u00f8\u00f4\3\2\2\2\u00f8\u00f9\3\2\2\2\u00f9\u00fb\3\2"+
		"\2\2\u00fa\u00fc\5 \21\2\u00fb\u00fa\3\2\2\2\u00fc\u00fd\3\2\2\2\u00fd"+
		"\u00fb\3\2\2\2\u00fd\u00fe\3\2\2\2\u00fe\37\3\2\2\2\u00ff\u0102\7\16\2"+
		"\2\u0100\u0102\5<\37\2\u0101\u00ff\3\2\2\2\u0101\u0100\3\2\2\2\u0102\u0104"+
		"\3\2\2\2\u0103\u0105\5&\24\2\u0104\u0103\3\2\2\2\u0105\u0106\3\2\2\2\u0106"+
		"\u0104\3\2\2\2\u0106\u0107\3\2\2\2\u0107\u010f\3\2\2\2\u0108\u010a\7$"+
		"\2\2\u0109\u010b\5\"\22\2\u010a\u0109\3\2\2\2\u010b\u010c\3\2\2\2\u010c"+
		"\u010a\3\2\2\2\u010c\u010d\3\2\2\2\u010d\u010f\3\2\2\2\u010e\u0101\3\2"+
		"\2\2\u010e\u0108\3\2\2\2\u010f!\3\2\2\2\u0110\u0112\7.\2\2\u0111\u0113"+
		"\5$\23\2\u0112\u0111\3\2\2\2\u0113\u0114\3\2\2\2\u0114\u0112\3\2\2\2\u0114"+
		"\u0115\3\2\2\2\u0115\u0116\3\2\2\2\u0116\u0117\7/\2\2\u0117#\3\2\2\2\u0118"+
		"\u0119\5<\37\2\u0119\u011a\7\20\2\2\u011a\u011b\5F$\2\u011b%\3\2\2\2\u011c"+
		"\u011d\5<\37\2\u011d\u011f\7.\2\2\u011e\u0120\5(\25\2\u011f\u011e\3\2"+
		"\2\2\u011f\u0120\3\2\2\2\u0120\u0121\3\2\2\2\u0121\u0122\7/\2\2\u0122"+
		"\'\3\2\2\2\u0123\u0128\5*\26\2\u0124\u0125\7\62\2\2\u0125\u0127\5*\26"+
		"\2\u0126\u0124\3\2\2\2\u0127\u012a\3\2\2\2\u0128\u0126\3\2\2\2\u0128\u0129"+
		"\3\2\2\2\u0129)\3\2\2\2\u012a\u0128\3\2\2\2\u012b\u012c\5<\37\2\u012c"+
		"\u012d\7\20\2\2\u012d\u012e\5,\27\2\u012e+\3\2\2\2\u012f\u0132\5F$\2\u0130"+
		"\u0132\5> \2\u0131\u012f\3\2\2\2\u0131\u0130\3\2\2\2\u0132-\3\2\2\2\u0133"+
		"\u0134\b\30\1\2\u0134\u0135\5<\37\2\u0135\u0137\7.\2\2\u0136\u0138\5("+
		"\25\2\u0137\u0136\3\2\2\2\u0137\u0138\3\2\2\2\u0138\u0139\3\2\2\2\u0139"+
		"\u013a\7/\2\2\u013a\u0142\3\2\2\2\u013b\u013c\7\34\2\2\u013c\u0142\5."+
		"\30\6\u013d\u013e\7.\2\2\u013e\u013f\5.\30\2\u013f\u0140\7/\2\2\u0140"+
		"\u0142\3\2\2\2\u0141\u0133\3\2\2\2\u0141\u013b\3\2\2\2\u0141\u013d\3\2"+
		"\2\2\u0142\u014b\3\2\2\2\u0143\u0144\f\5\2\2\u0144\u0145\7\5\2\2\u0145"+
		"\u014a\5.\30\6\u0146\u0147\f\4\2\2\u0147\u0148\7\36\2\2\u0148\u014a\5"+
		".\30\5\u0149\u0143\3\2\2\2\u0149\u0146\3\2\2\2\u014a\u014d\3\2\2\2\u014b"+
		"\u0149\3\2\2\2\u014b\u014c\3\2\2\2\u014c/\3\2\2\2\u014d\u014b\3\2\2\2"+
		"\u014e\u014f\7\r\2\2\u014f\u0151\5<\37\2\u0150\u0152\5@!\2\u0151\u0150"+
		"\3\2\2\2\u0151\u0152\3\2\2\2\u0152\61\3\2\2\2\u0153\u0154\7\3\2\2\u0154"+
		"\u0155\5<\37\2\u0155\u0157\7.\2\2\u0156\u0158\5\66\34\2\u0157\u0156\3"+
		"\2\2\2\u0157\u0158\3\2\2\2\u0158\u0159\3\2\2\2\u0159\u015b\7/\2\2\u015a"+
		"\u015c\5@!\2\u015b\u015a\3\2\2\2\u015b\u015c\3\2\2\2\u015c\63\3\2\2\2"+
		"\u015d\u015e\7 \2\2\u015e\u015f\5<\37\2\u015f\u0161\7.\2\2\u0160\u0162"+
		"\5\66\34\2\u0161\u0160\3\2\2\2\u0161\u0162\3\2\2\2\u0162\u0163\3\2\2\2"+
		"\u0163\u0165\7/\2\2\u0164\u0166\5@!\2\u0165\u0164\3\2\2\2\u0165\u0166"+
		"\3\2\2\2\u0166\65\3\2\2\2\u0167\u016c\58\35\2\u0168\u0169\7\62\2\2\u0169"+
		"\u016b\58\35\2\u016a\u0168\3\2\2\2\u016b\u016e\3\2\2\2\u016c\u016a\3\2"+
		"\2\2\u016c\u016d\3\2\2\2\u016d\67\3\2\2\2\u016e\u016c\3\2\2\2\u016f\u0170"+
		"\5:\36\2\u0170\u0171\5<\37\2\u01719\3\2\2\2\u0172\u0173\t\2\2\2\u0173"+
		";\3\2\2\2\u0174\u0175\79\2\2\u0175=\3\2\2\2\u0176\u0178\7\64\2\2\u0177"+
		"\u0176\3\2\2\2\u0177\u0178\3\2\2\2\u0178\u0179\3\2\2\2\u0179\u017e\5<"+
		"\37\2\u017a\u017b\7\63\2\2\u017b\u017d\5<\37\2\u017c\u017a\3\2\2\2\u017d"+
		"\u0180\3\2\2\2\u017e\u017c\3\2\2\2\u017e\u017f\3\2\2\2\u017f?\3\2\2\2"+
		"\u0180\u017e\3\2\2\2\u0181\u0182\7-\2\2\u0182A\3\2\2\2\u0183\u0184\7\66"+
		"\2\2\u0184C\3\2\2\2\u0185\u0186\5B\"\2\u0186\u0187\7\63\2\2\u0187\u0188"+
		"\5B\"\2\u0188E\3\2\2\2\u0189\u018a\7-\2\2\u018aG\3\2\2\2\u018b\u018c\5"+
		"B\"\2\u018c\u018d\t\3\2\2\u018dI\3\2\2\2\63NU\\bilpt{~\u0085\u0090\u0099"+
		"\u009f\u00a4\u00a9\u00af\u00b7\u00be\u00c9\u00ce\u00d2\u00d7\u00df\u00e1"+
		"\u00ec\u00f2\u00f8\u00fd\u0101\u0106\u010c\u010e\u0114\u011f\u0128\u0131"+
		"\u0137\u0141\u0149\u014b\u0151\u0157\u015b\u0161\u0165\u016c\u0177\u017e";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}