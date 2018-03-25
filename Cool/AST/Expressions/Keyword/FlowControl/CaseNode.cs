using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class CaseNode : KeywordNode
    {
        public ExpressionNode ExpressionCase { get; set; }
        public List<(FormalNode Formal, ExpressionNode Expression)> Branches { get; set; }
        public int BranchSelected { get; set; }

        public CaseNode(ParserRuleContext context) : base(context)
        {
            Branches = new List<(FormalNode, ExpressionNode)>();
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }
    }
}
