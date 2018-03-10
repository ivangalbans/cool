using Antlr4.Runtime;

namespace Cool.AST
{
    class IfNode : KeywordNode
    {
        public ExpressionNode Condition => Children[0] as ExpressionNode;
        public ExpressionNode Body => Children[1] as ExpressionNode;
        public ExpressionNode ElseBody => Children[2] as ExpressionNode;

        public IfNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
