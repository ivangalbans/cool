using Antlr4.Runtime;

namespace Cool.AST
{
    class ObjectNode : ExpressionNode
    {
        public ObjectNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
