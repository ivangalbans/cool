using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class IsVoidNode : UnaryOperationNode
    {
        public IsVoidNode(ParserRuleContext context) : base(context)
        {
            Type = Types.Void;
        }

    }
}
