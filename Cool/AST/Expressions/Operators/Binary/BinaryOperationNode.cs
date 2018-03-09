using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class BinaryOperationNode : ExpressionNode
    {
        public ExpressionNode LeftOperand => Children[0] as ExpressionNode;
        public ExpressionNode RightOperand => Children[1] as ExpressionNode;

        public BinaryOperationNode(ParserRuleContext context) : base(context) { }

    }
}
