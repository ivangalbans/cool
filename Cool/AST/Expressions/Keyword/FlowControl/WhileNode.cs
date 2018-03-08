using Antlr4.Runtime;

namespace Cool.AST
{
    class WhileNode : KeywordNode
    {
        public ExpressionNode Condition => Children[0] as ExpressionNode;
        public ExpressionNode Body => Children[1] as ExpressionNode;

        public WhileNode(ParserRuleContext context) : base(context) { }

    }
}
