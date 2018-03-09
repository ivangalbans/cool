using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class StringNode : AtomNode
    {
        public string Text { get; set; }

        public StringNode(int line, int column, string text) : base(line, column)
        {
            Type = Types.String;
            Text = text;
        }
    }
}
