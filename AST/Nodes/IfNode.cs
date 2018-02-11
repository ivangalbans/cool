using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class IfNode : KeywordNode
    {
        public ExpressionNode Condition { get; set; }
        public ExpressionNode Body { get; set; }
        public ExpressionNode ElseBody { get; set; }

        public IfNode(ExpressionNode condition, ExpressionNode body, ExpressionNode elseBody)
        {
            Condition = condition;
            Body = body;
            ElseBody = elseBody;
        }
    }
}
