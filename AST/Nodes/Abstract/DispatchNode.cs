using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grammars;

namespace AST.Nodes.Abstract
{
    public abstract class DispatchNode : ASTNode
    {
        public abstract string TextID { get; set; }

        public abstract int LineID { get; set; }

        public abstract int ColumnID { get; set; }

        public abstract List<ExpressionNode> ExpressionParams { get; set; }

        public DispatchNode(Token id, List<ExpressionNode> expressionParams)
        {
            TextID = id.Text;
            LineID = id.Line;
            ColumnID = id.Column;
            ExpressionParams = expressionParams;
        }
    }
}
