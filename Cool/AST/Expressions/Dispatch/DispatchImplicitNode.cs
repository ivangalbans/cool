using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class DispatchImplicitNode : DispatchNode
    {
        public override string TextID { get; set; }

        public override int LineID { get; set; }

        public override int ColumnID { get; set; }

        public override List<ExpressionNode> ExpressionParams { get; set; }


        public DispatchImplicitNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
