using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class ComparisonOperation : BinaryOperationNode
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public ComparisonOperation(ParserRuleContext context) : base(context) { }
    }
}
