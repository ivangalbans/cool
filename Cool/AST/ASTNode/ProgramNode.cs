using Antlr4.Runtime;

namespace Cool.AST
{
    internal class ProgramNode : ASTNode
    {
        public ProgramNode(ParserRuleContext context) : base(context) { }
    }
}