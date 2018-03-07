using Antlr4.Runtime;

namespace Cool.AST
{
    class LessEqual : ComparisonOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public LessEqual(ParserRuleContext context) : base(context) { }

    }
}
