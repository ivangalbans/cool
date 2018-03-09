using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class FormalNode : AuxiliaryNode
    {
        public IdentifierNode Id => Children[0] as IdentifierNode;
        public IdentifierNode TypeId => Children[1] as IdentifierNode;

        public FormalNode(ParserRuleContext context) : base(context) { }
    }
}
