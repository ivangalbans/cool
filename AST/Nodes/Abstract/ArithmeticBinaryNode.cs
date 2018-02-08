using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Nodes.Abstract
{
    public abstract class ArithmeticBinaryNode : BinaryNode
    {
        public ArithmeticBinaryNode(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }
    }
}
