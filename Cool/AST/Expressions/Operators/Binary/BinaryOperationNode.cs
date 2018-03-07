using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class BinaryOperationNode : ExpressionNode
    {
        public abstract ExpressionNode LeftExpression {get; set; }
        public abstract ExpressionNode RightExpression { get; set; }

        public BinaryOperationNode(ParserRuleContext context) : base(context) { }

    }
}
