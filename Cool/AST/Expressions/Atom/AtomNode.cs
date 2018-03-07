using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class AtomNode : ExpressionNode
    {
        public AtomNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
