using Antlr4.Runtime;

namespace Cool.AST
{
    class EqualNode : ComparisonOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public EqualNode(ParserRuleContext context) : base(context) { }
        
    }
}