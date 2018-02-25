using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using AST.Scope;
using AST.Visitor;

namespace AST.Nodes
{
    public class DivNode : ArithmeticOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public DivNode(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }

    }
}
