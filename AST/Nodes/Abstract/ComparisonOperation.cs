using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Nodes.Abstract
{
    public class ComparisonOperation : BinaryNode
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public ComparisonOperation(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }
    }
}
