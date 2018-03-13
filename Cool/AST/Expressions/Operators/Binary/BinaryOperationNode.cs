using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class BinaryOperationNode : ExpressionNode
    {
        public ExpressionNode LeftOperand { get; set; }
        public ExpressionNode RightOperand { get; set; }

        public BinaryOperationNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
