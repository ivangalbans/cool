
using Antlr4.Runtime;

namespace Cool.AST
{
    class IdentifierNode : AtomNode
    {
        public string Text { get; set; }

        public IdentifierNode(ParserRuleContext context) : base(context) { }

        public IdentifierNode(int line, int column, string text) : base(line, column)
        {
            Text = text;
        }
    }
}
