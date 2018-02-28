﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using AST.Scope;
using AST.Visitor;

namespace AST.Nodes
{
    public class IsVoidNode : UnaryOperationNode
    {
        public override ExpressionNode Expression { get; set; }

        public IsVoidNode(ExpressionNode expression) : base(expression) { }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}