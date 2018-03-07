using Antlr4.Runtime;

namespace Cool.AST
{
    class StringNode : AtomNode
    {
        public string Value { get; set; }

        public StringNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
