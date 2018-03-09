using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class ClassNode : ASTNode
    {
        public IdentifierNode TypeClass => Children[0] as IdentifierNode;

        public IdentifierNode TypeInherits => Children[1] as IdentifierNode;

        public List<FeatureNode> FeatureNodes { get; set; }

        public ClassNode(ParserRuleContext context) : base(context)
        {
            FeatureNodes = new List<FeatureNode>();
        }

    }
}
