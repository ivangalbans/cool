using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class BinaryOperationNode : ExpressionNode
    {
        public ExpressionNode LeftExpression => Children[0] as ExpressionNode;
        public ExpressionNode RightExpression => Children[1] as ExpressionNode;

        public BinaryOperationNode(ParserRuleContext context) : base(context) { }

    }
}
