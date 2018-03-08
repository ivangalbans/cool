using Antlr4.Runtime;

namespace Cool.AST
{
    class IdentifierNode : AuxiliaryNode
    {
        public string Name { get; set; }

        public IdentifierNode(ParserRuleContext context, string name) : base(context)
        {
            Name = name;
        }

        public IdentifierNode(int line, int column, string name) : base(line, column)
        {
            Name = name;
        }
        
    }
}
