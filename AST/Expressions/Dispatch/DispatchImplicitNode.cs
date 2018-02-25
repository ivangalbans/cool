using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class DispatchImplicitNode : DispatchNode
    {
        public override string TextID { get; set; }

        public override int LineID { get; set; }

        public override int ColumnID { get; set; }

        public override List<ExpressionNode> ExpressionParams { get; set; }


        public DispatchImplicitNode(Token id, List<ExpressionNode> expression) : base(id, expression) { }
    }
}
