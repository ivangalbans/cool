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
    public class AddNode : ArithmeticOperation
    {
        public override ExpressionNode LeftExpression { get;  set; }
        public override ExpressionNode RightExpression { get; set; }

        public AddNode(ExpressionNode leftNode, ExpressionNode rightNode) : base(leftNode, rightNode) { }

    }
}
