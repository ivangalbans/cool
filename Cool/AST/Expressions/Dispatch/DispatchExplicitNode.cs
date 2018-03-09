using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class DispatchExplicitNode : DispatchNode
    {
        public ExpressionNode Expression => Children[0] as ExpressionNode;
        public IdentifierNode IdType => Children[1] as IdentifierNode;
        public DispatchImplicitNode Method => Children[2] as DispatchImplicitNode;

        public DispatchExplicitNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
