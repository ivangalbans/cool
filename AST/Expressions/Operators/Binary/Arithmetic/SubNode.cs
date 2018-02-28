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
    public class SubNode : ArithmeticOperation
    {
        public override ExpressionNode LeftExpression { get; set; }
        public override ExpressionNode RightExpression { get; set; }

        public SubNode(ExpressionNode leftExp, ExpressionNode rightExp) : base(leftExp, rightExp) { }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }

    }
}