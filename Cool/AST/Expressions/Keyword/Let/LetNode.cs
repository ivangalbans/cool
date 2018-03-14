using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class LetNode : KeywordNode
    {
        public List<AttributeNode> Initialization { get; set; }
        public ExpressionNode ExpressionBody { get; set; }

        public LetNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }

    }
}
