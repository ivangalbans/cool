using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class UnaryOperationNode : ExpressionNode
    {
        public ExpressionNode Operand => Children[0] as ExpressionNode;

        public UnaryOperationNode(ParserRuleContext context) : base(context) { }
    }
}
