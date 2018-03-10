using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class IsVoidNode : UnaryOperationNode
    {
        public override string OperatorName => "isvoid operation";

        public IsVoidNode(ParserRuleContext context) : base(context)
        {
            Type = Types.Void;
        }

    }
}
