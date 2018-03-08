using Antlr4.Runtime;

namespace Cool.AST
{
    class NotNode : UnaryOperationNode
    {
        public override ExpressionNode Expression => Children[0] as ExpressionNode;

        public NotNode(ParserRuleContext context) : base(context) { }

    }
}
