using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    class AddNode : ArithmeticOperation
    {
        public AddNode(ParserRuleContext context) : base(context)
        {
        }

        public override string Symbol => "+";

        public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }
    }
}
