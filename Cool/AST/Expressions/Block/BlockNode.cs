using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class BlockNode : ExpressionNode
    {
        public List<ExpressionNode> ExpressionsBlock { get; set; }

        public BlockNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }
    }
}
