using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class WhileNode : ExpressionNode
    {
        public ExpressionNode Condition { get; set; }
        public ExpressionNode Body { get; set; }

        public WhileNode(ExpressionNode condition, ExpressionNode body)
        {
            Condition = condition;
            Body = body;
        }
    }
}
