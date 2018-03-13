using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class FormalNode : AuxiliaryNode
    {
        public IdNode Id { get; set; }
        public TypeNode TypeId { get; set; }

        public FormalNode(ParserRuleContext context) : base(context)
        {
        }
    }
}
