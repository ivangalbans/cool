using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class ClassNode : ASTNode
    {
        public IdNode TypeClass => Children[0] as IdNode;

        public IdNode TypeInherits => Children[1] as IdNode;

        public List<FeatureNode> FeatureNodes { get; set; }

        public ClassNode(ParserRuleContext context) : base(context)
        {
            FeatureNodes = new List<FeatureNode>();
        }

    }
}
