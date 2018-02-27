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
    public class BlockNode : ExpressionNode
    {
        public List<ExpressionNode> Expressions { get; set; }

        public BlockNode(List<ExpressionNode> expressions)
        {
            Expressions = expressions;
        }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}
