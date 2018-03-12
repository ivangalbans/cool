using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class StringNode : AtomNode
    {
        public string Text { get; set; }

        public StringNode(ParserRuleContext context, string text) : base(context)
        {
            Text = text;
        }
    }
}
