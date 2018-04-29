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

        public void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            string repr = $"Program Node, Number of Classes: {Classes.Count}:\n";
            foreach (var c in Classes)
            {
                repr += c.ToString() + "\n";
            }
            return repr.Replace("\n","\n| ");
        }
    }
}