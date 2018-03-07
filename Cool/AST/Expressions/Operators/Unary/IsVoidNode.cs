using Antlr4.Runtime;

namespace Cool.AST
{
    class IsVoidNode : UnaryOperationNode
    {
        public override ExpressionNode Expression { get; set; }

        public IsVoidNode(ParserRuleContext context) : base(context) { }

    }
}
