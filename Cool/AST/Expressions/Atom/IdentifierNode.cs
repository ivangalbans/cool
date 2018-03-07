using Antlr4.Runtime;

namespace Cool.AST
{
    class IdentifierNode : AtomNode
    {
        public string Name { get; set; }

        public IdentifierNode(ParserRuleContext context) : base(context)
        {

        }
        
    }
}
