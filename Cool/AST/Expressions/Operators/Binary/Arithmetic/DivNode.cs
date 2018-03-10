using Antlr4.Runtime;

namespace Cool.AST
{
    class DivNode : ArithmeticOperation
    {
        public DivNode(ParserRuleContext context) : base(context)
        {
        }

        public override string OperatorName => "divide";

    }
}
