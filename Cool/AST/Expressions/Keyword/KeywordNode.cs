using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class KeywordNode : ExpressionNode
    {
        public KeywordNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
