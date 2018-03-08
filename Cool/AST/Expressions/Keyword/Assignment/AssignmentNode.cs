using Antlr4.Runtime;

namespace Cool.AST
{
    class AssignmentNode : ExpressionNode
    {
        public string TextID { get; set; }
        public ExpressionNode Expression => Children[0] as ExpressionNode;

        public AssignmentNode(ParserRuleContext context, string textId) : base(context)
        {
            TextID = textId;
        }

    }
}
