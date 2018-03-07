using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class ExpressionNode : ASTNode
    {
        public ExpressionNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
