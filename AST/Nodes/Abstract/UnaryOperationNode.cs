using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Nodes.Abstract
{
    public abstract class UnaryOperationNode : ExpressionNode
    {
        public abstract ExpressionNode Expression { get; set; }

        public UnaryOperationNode(ExpressionNode exp)
        {
            Expression = exp;
        }
    }
}
