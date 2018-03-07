using Antlr4.Runtime;

namespace Cool.AST
{
    class NotNode : UnaryOperationNode
    {
        public override ExpressionNode Expression { get; set; }

        public NotNode(ParserRuleContext context) : base(context) { }

    }
}
