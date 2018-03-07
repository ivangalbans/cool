using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class DispatchExplicitNode : DispatchNode
    {
        public override string TextID { get; set; }

        public override int LineID { get; set; }
        public override int ColumnID { get; set; }

        public override List<ExpressionNode> ExpressionParams { get; set; }

        public ExpressionNode Expression { get; set; }

        public string TextType { get; set; }

        public int LineType { get; set; }
        public int ColumnType { get; set; }

        public DispatchExplicitNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
