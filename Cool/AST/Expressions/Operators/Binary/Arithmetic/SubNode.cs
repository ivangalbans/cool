using Antlr4.Runtime;

namespace Cool.AST
{
    class SubNode : ArithmeticOperation
    {
        public SubNode(ParserRuleContext context) : base(context) { }

        public override string OperatorName => "minus";

    }
}
