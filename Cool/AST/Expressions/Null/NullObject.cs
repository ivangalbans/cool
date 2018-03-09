
using Antlr4.Runtime;

namespace Cool.AST
{
    class NullObject : ASTNode
    {
        public NullObject(ParserRuleContext context) : base(context) { }

        public NullObject(int line, int column) : base(line, column) { }
    }
}
