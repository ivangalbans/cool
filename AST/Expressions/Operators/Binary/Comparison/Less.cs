using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;

namespace AST.Nodes
{
    public class Less : ComparisonOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public Less(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }
    }
}
