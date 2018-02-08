using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class EqualNode : ComparisonOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public EqualNode(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }
    }
}