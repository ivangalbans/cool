using System.Collections.Generic;

namespace Cool.AST
{
    class ClassNode : ASTNode
    {
        public string Text { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public string TextInherits { get; set; }

        public int LineInherits { get; set; }

        public int ColumnInherits { get; set; }

        public List<FeatureNode> FeatureNodes { get; set; }

        public ClassNode(ParserRuleContext context) : base(context)
        {
            Text = type.Text;
            Line = type.Line;
            Column = type.Column;
            TextInherits = inherits?.Text;
            LineInherits = inherits?.Line ?? 0;
            ColumnInherits = inherits?.Column ?? 0;
            FeatureNodes = new List<FeatureNode>(featuresNodes);
        }

        public void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}
