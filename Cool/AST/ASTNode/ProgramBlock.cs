using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.AST
{
    class ProgramBlock : ProgramNode
    {
        public List<ClassNode> ClassNodes => Children.Cast<ClassNode>().ToList();
        public ProgramBlock(ParserRuleContext context) : base(context)
        {
        }
    }
}
