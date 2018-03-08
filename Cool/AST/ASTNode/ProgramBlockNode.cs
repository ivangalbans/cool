using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class ProgramBlockNode : ProgramNode
    {
        private List<ClassNode> _classNode = new List<ClassNode>();
        public List<ClassNode> ClassNodes => Children.Select(x => x as ClassNode).ToList();

        public ProgramBlockNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
