using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class ProgramNode : ASTNode, IVisit
    {
        public List<ClassNode> Classes { get; set; }

        public ProgramNode(ParserRuleContext context) : base(context)
        {
        }

        public void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
        {
            visitor.Visit(this, scope, errors);
        }
    }
}