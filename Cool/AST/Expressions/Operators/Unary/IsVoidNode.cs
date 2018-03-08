using Antlr4.Runtime;

namespace Cool.AST
{
    class IsVoidNode : UnaryOperationNode
    {
        public override ExpressionNode Expression => Children[0] as ExpressionNode;

        public IsVoidNode(ParserRuleContext context) : base(context) { }

    }
}
