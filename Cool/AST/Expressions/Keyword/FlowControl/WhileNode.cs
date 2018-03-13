using Antlr4.Runtime;

namespace Cool.AST
{
    class WhileNode : KeywordNode
    {
        public ExpressionNode Condition { get; set; }
        public ExpressionNode Body { get; set; }

        public WhileNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
