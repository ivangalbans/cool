using Antlr4.Runtime;

namespace Cool.AST
{
    class AuxiliaryNode : ExpressionNode
    {
        public AuxiliaryNode(ParserRuleContext context) : base(context) { }

        public AuxiliaryNode(int line, int column) : base(line, column) { }

    }
}
