using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class IntNode : AtomNode
    {
        public int Value { get; set; }

        public IntNode(ParserRuleContext context, string text) : base(context)
        {
            Value = int.Parse(text);
        }

    }
}
