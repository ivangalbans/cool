using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class DispatchExplicitNode : DispatchNode
    {

        //public override List<ExpressionNode> Arguments { get; set; }


        public DispatchExplicitNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
