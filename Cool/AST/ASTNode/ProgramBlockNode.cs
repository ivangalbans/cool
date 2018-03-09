using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class ProgramBlockNode : ProgramNode
    {
        public List<ClassNode> ClassNodes { get; set; }

        public ProgramBlockNode(ParserRuleContext context) : base(context)
        {
            ClassNodes = new List<ClassNode>();
        }
    }
}
