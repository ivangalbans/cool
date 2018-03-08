using Antlr4.Runtime;

namespace Cool.AST
{
    class AddNode : ArithmeticOperation
    {
        public AddNode(ParserRuleContext context) : base(context) { }

        public override string OperatorName => "plus";

    }
}
