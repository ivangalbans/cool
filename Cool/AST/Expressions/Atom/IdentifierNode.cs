﻿
using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class IdentifierNode : AtomNode
    {
        public string Text { get; set; }

        public IdentifierNode(ParserRuleContext context) : base(context) { }

        public IdentifierNode(int line, int column, string text) : base(line, column)
        {
            Text = text;
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }
    }
}
