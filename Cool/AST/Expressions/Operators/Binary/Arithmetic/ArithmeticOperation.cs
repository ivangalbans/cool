using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class ArithmeticOperation : BinaryOperationNode
    {
        public ArithmeticOperation(ParserRuleContext context) : base(context) { }

        public abstract string OperatorName { get; }
    }
}
