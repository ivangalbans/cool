using Antlr4.Runtime;

namespace Cool.AST
{
    class AssignmentNode : ExpressionNode
    {
        public IdNode ID => Children[0] as IdNode;
        public ExpressionNode ExpressionRight => Children[1] as ExpressionNode;

        public AssignmentNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
