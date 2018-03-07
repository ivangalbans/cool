using Antlr4.Runtime;

namespace Cool.AST
{
    class Less : ComparisonOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public Less(ParserRuleContext context) : base(context) { }
        
    }
}
