using Antlr4.Runtime;

namespace Cool.AST
{
    class BoolNode : AtomNode
    {
        public bool Value { get; set; }

        public BoolNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
