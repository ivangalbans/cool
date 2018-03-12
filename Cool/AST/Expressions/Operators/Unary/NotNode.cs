using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class NotNode : UnaryOperationNode
    {
        public override string OperatorName => "operation not";

        public NotNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
