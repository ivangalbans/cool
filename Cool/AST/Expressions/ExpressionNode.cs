using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class ExpressionNode : ASTNode
    {
        public ExpressionNode(ParserRuleContext context) : base(context) { }

        public ExpressionNode(int line, int column) : base(line, column) { }
    }
}
