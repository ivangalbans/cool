using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class UnaryOperationNode : ExpressionNode
    {
        public abstract ExpressionNode Expression { get;}

        public UnaryOperationNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
