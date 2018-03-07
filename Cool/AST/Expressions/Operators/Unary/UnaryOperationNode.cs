using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class UnaryOperationNode : ExpressionNode
    {
        public abstract ExpressionNode Expression { get; set; }

        public UnaryOperationNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
