using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class BlockNode : ExpressionNode
    {
        public List<ExpressionNode> Expressions { get; set; }

        public BlockNode(List<ExpressionNode> expressions)
        {
            Expressions = expressions;
        }
    }
}
