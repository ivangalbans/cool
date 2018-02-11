using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class AttributeNode : FeatureNode
    {
        public ExpressionNode Expression { get; set; }

        public CoolType TypeVar { get; set; }

        public string TextType { get; set; }

        public int LineType { get; set; }

        public int ColumnType { get; set; }


        public AttributeNode(Token id, Token type, ExpressionNode exppression)
        {
            TextID = id.Text;
            LineID = id.Line;
            ColumnID = id.Column;
            TextType = type.Text;
            LineType = type.Line;
            ColumnType = type.Column;
            Expression = exppression;
        }
    }
}
