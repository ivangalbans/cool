using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    internal class ProgramNode : ASTNode
    {
        public ProgramBlock ProgramBlock => Children[0] as ProgramBlock;

        public ProgramNode(ParserRuleContext context) : base(context)
        {
        }
    }
}