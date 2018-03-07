using Antlr4.Runtime;

namespace Cool.AST
{
    class SubNode : ArithmeticOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public SubNode(ParserRuleContext context) : base(context) { }

        public override string OperatorName => "minus";

    }
}
