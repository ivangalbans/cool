using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public abstract class FeatureNode : ASTNode, IVisit
    {
        public FeatureNode(ParserRuleContext context) : base(context) { }

        public abstract void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors);
    }
}
