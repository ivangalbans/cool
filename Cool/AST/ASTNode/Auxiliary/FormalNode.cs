using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class FormalNode : AuxiliaryNode
    {
        public IdNode Id { get; set; }
        public TypeNode TypeId { get; set; }

        public FormalNode(ParserRuleContext context) : base(context)
        {
        }
    }
}
