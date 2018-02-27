using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AST.Nodes.Abstract;
using AST.Scope;
using AST.Visitor;

namespace AST.Nodes
{
    public class EqualNode : ComparisonOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public EqualNode(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}