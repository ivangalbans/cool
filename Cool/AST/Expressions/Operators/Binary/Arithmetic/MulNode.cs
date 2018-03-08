using Antlr4.Runtime;

namespace Cool.AST
{
    class MulNode : ArithmeticOperation
    {

        public MulNode(ParserRuleContext context) : base(context) { }

        public override string OperatorName => "multiply";

    }
}
