using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class FormalNode : AuxiliaryNode
    {
        public IdNode Id => Children[0] as IdNode;
        public TypeNode TypeId => Children[1] as TypeNode;

        public FormalNode(ParserRuleContext context) : base(context)
        {
        }
    }
}
