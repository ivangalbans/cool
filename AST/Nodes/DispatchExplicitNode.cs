using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class DispatchExplicitNode : DispatchNode
    {
        public override string TextID { get; set; }

        public override int LineID { get; set; }
        public override int ColumnID { get; set; }

        public override List<ExpressionNode> ExpressionParams { get; set; }

        public ExpressionNode Expression { get; set; }

        public string TextType { get; set; }

        public int LineType { get; set; }
        public int ColumnType { get; set; }

        public DispatchExplicitNode(ExpressionNode expression, Token type, Token id, List<ExpressionNode> expressionParams) : base(id, expressionParams)
        {
            Expression = expression;
            TextType = type?.Text;
            LineType = type is null ? 0 : type.Line;
            ColumnType = type is null ? 0 : type.Column;
        }

    }
}
