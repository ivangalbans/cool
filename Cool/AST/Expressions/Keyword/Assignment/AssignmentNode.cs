using Antlr4.Runtime;

namespace Cool.AST
{
    class AssignmentNode : ExpressionNode
    {
        public IdentifierNode ID => Children[0] as IdentifierNode;
        public ExpressionNode Expression => Children[1] as ExpressionNode;

        public AssignmentNode(ParserRuleContext context) : base(context) { }

    }
}
