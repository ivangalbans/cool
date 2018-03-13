using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    internal class ProgramNode : ASTNode
    {
        public List<ClassNode> Classes { get; set; }

        public ProgramNode(ParserRuleContext context) : base(context)
        {
        }

        public override void CheckSemantics(IScope scope, List<SemanticError> errors)
        {
            var tour1 = new Tour1();
            tour1.Visit(this, scope, errors);
        }

    }
}