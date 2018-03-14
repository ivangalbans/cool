﻿using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    class SubNode : ArithmeticOperation
    {
        public SubNode(ParserRuleContext context) : base(context)
        {
        }

        public override string OperatorName => "minus";

        public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }
    }
}
