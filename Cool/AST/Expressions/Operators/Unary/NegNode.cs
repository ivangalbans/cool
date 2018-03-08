using Antlr4.Runtime;

namespace Cool.AST
{
    class NegNode : UnaryOperationNode
    {
        public override ExpressionNode Expression => Children[0] as ExpressionNode;

        public NegNode(ParserRuleContext context) : base(context) { }

    }
}
