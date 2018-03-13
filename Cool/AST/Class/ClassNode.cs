using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class ClassNode : ASTNode
    {
        public TypeNode TypeClass { get; set; }

        public TypeNode TypeInherit { get; set; }

        public List<FeatureNode> FeatureNodes { get; set; }

        public IScope Scope { get; set; }

        public ClassNode(ParserRuleContext context) : base(context)
        {
        }

        public ClassNode() : base(0, 0)
        {

        }
    }
}
