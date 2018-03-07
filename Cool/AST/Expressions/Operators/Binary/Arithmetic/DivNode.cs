using Antlr4.Runtime;

namespace Cool.AST
{
    class DivNode : ArithmeticOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public DivNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
