using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class IsVoidNode : UnaryOperationNode
    {
        public override ExpressionNode Expression { get; set; }

        public IsVoidNode(ExpressionNode exp) : base(exp) { }
    }
}
