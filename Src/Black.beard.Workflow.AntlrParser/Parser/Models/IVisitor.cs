namespace Bb.Workflows.Parser.Models
{
    public interface IVisitor<T>
    {

        T VisitRule(RuleDefinitionModel m);

        T VisitBinary(BinaryExpressionModel m);

        T VisitConstant(ConstantExpressionModel m);

        T VisitNot(NotExpressionModel m);

        T VisitRuleExpression(RuleExpressionModel m);
    
    }
}