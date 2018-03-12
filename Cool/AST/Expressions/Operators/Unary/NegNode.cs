using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class NegNode : UnaryOperationNode
    {
        public override string OperatorName => "negative operation";

        public NegNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
