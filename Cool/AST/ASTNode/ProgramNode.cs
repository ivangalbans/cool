using Antlr4.Runtime;
using System.Collections.Generic;

namespace Cool.AST
{
    internal class ProgramNode : ASTNode
    {
        public List<ClassNode> ClassNodes { get; set; }

        public ProgramNode(ParserRuleContext context) : base(context)
        {
            ClassNodes = new List<ClassNode>();
        }
    }
}