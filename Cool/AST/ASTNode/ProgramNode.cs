using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    internal class ProgramNode : ASTNode
    {
        public List<ClassNode> ClassNodes => Children.Cast<ClassNode>().ToList();

        public ProgramNode(ParserRuleContext context) : base(context)
        {
        }

    }
}