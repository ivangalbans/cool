using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class ProgramNode : ASTNode
    {
        public List<ClassNode> ClassNodes { get; set; }

        public ProgramNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
