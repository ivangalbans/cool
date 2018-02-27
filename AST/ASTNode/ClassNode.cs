using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;
using AST.Visitor;
using AST.Scope;

namespace AST.Nodes
{
    public class ClassNode : ASTNode, IVisit
    {
        public string Text { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public string TextInherits { get; set; }

        public int LineInherits { get; set; }

        public int ColumnInherits { get; set; }

        public List<FeatureNode> FeatureNodes { get; set; }

        public ClassNode(Token type, Token inherits, IEnumerable<FeatureNode> featuresNodes)
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
