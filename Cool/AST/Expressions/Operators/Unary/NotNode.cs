using Antlr4.Runtime;

namespace Cool.AST
{
    class NotNode : UnaryOperationNode
    {

        public NotNode(ParserRuleContext context) : base(context) { }

    }
}
