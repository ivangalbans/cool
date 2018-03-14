using Antlr4.Runtime;

namespace Cool.AST
{
    public abstract class KeywordNode : ExpressionNode
    {
        public KeywordNode(ParserRuleContext context) : base(context)
        {
        }
    }
}
