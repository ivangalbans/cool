using Antlr4.Runtime;

namespace Cool.AST
{
    class IfNode : KeywordNode
    {
        public ExpressionNode Condition { get; set; }
        public ExpressionNode Body { get; set; }
        public ExpressionNode ElseBody { get; set; }

        public IfNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
