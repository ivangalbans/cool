using Antlr4.Runtime;

namespace Cool.AST
{
    class BoolNode : AtomNode
    {
        public bool Value { get; set; }

        public BoolNode(ParserRuleContext context, string text) : base(context)
        {
            Value = bool.Parse(text);
        }
    }
}
