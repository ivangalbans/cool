using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Nodes.Abstract
{
    public abstract class BinaryOperationNode : ExpressionNode
    {
        public abstract ExpressionNode LeftExpression {get; set; }
        public abstract ExpressionNode RightExpression { get; set; }

        public BinaryOperationNode(ExpressionNode leftExp, ExpressionNode rightExp)
        {
            LeftExpression = leftExp;
            RightExpression = rightExp;
        }
    }
}
