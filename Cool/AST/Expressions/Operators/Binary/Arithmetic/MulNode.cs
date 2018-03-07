using Antlr4.Runtime;

namespace Cool.AST
{
    class MulNode : ArithmeticOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public MulNode(ParserRuleContext context) : base(context) { }

    }
}
