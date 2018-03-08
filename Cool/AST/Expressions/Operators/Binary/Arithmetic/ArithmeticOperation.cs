using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    abstract class ArithmeticOperation : BinaryOperationNode
    {
        public ArithmeticOperation(ParserRuleContext context) : base(context)
        {
            Type = Types.Int;
        }

        public abstract string OperatorName { get; }
    }
}
