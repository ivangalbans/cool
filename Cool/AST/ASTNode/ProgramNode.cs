using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    internal class ProgramNode : ASTNode
    {
        public List<ClassNode> Classes => Children.Cast<ClassNode>().ToList();

        public ProgramNode(ParserRuleContext context) : base(context)
        {
        }

    }
}