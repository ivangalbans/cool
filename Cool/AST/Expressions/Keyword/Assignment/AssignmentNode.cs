using Antlr4.Runtime;

namespace Cool.AST
{
    class AssignmentNode : ExpressionNode
    {
        public string TextID { get; set; }
        public ExpressionNode Expression { get; set; }

        public AssignmentNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
