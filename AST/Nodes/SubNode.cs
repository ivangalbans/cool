using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class SubNode : ArithmeticBinaryNode
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public SubNode(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }
    }
}
