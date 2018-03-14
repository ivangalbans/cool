using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class AssignmentNode : ExpressionNode
    {
        public IdNode ID { get; set; }
        public ExpressionNode ExpressionRight { get; set; }

        public AssignmentNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }

    }
}
