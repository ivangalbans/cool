using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class ClassNode : ASTNode
    {
        public string TextId { get; set; }

        public string TextIdInherits { get; set; }

        public int LineInherits { get; set; }

        public int ColumnInherits { get; set; }

        public List<FeatureNode> FeatureNodes { get; set; }

        public ClassNode(ParserRuleContext context) :base(context)
        {
            
        }

    }
}
