using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class MethodNode : FeatureNode
    {
        public CoolType TypeReturn { get; set; }

        public string TextTypeReturn { get; set; }

        public int LineTypeReturn { get; set; }

        public int ColumnTypeReturn { get; set; }

        public List<(string TextID, string TextType)> parameters { get; set; }

        public List<((int LineID, int ColumnID), (int LineType, int ColumnType))> LineColumnFormals { get; set; }

        public ExpressionNode Expression { get; set; }

        public MethodNode(Token id, IEnumerable<(Token id, Token type)> paramsCollection, Token type, ExpressionNode expression)
        {
            TextID = id.Text;
            LineID = id.Line;
            ColumnID = id.Column;
            parameters = paramsCollection.Select(x => (x.id.Text, x.type.Text)).ToList();
            TextTypeReturn = type.Text;
            LineTypeReturn = type.Line;
            ColumnTypeReturn = type.Column;
            LineColumnFormals = paramsCollection.Select(x => ((x.id.Line, x.id.Column), (x.type.Line, x.type.Column))).ToList();
            Expression = expression;
        }
    }
}
