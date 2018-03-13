using Antlr4.Runtime;

namespace Cool.AST
{
    class AssignmentNode : ExpressionNode
    {
        public IdNode ID { get; set; }
        public ExpressionNode ExpressionRight { get; set; }

        public AssignmentNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
