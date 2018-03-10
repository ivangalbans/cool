using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class UnaryOperationNode : ExpressionNode
    {
        public ExpressionNode Operand => Children[0] as ExpressionNode;

        public abstract string OperatorName { get; }

        public UnaryOperationNode(ParserRuleContext context) : base(context)
        {
        }
    }
}
