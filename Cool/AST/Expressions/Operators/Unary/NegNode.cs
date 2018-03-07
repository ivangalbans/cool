using Antlr4.Runtime;

namespace Cool.AST
{
    class NegNode : UnaryOperationNode
    {
        public override ExpressionNode Expression { get; set; }

        public NegNode(ParserRuleContext context) : base(context) { }

    }
}
