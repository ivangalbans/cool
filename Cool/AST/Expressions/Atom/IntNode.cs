using Antlr4.Runtime;

namespace Cool.AST
{
    class IntNode : AtomNode
    {
        public int Value { get; set; }

        public IntNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
