using Antlr4.Runtime;

namespace Cool.AST
{
    class AddNode : ArithmeticOperation
    {
        public override ExpressionNode LeftExpression { get;  set; }
        public override ExpressionNode RightExpression { get; set; }

        public AddNode(ParserRuleContext context) : base(context)
        {

        }

        public override string OperatorName => "plus";

    }
}
