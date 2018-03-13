using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class DispatchExplicitNode : DispatchNode
    {
        public ExpressionNode Expression { get; set; }
        public TypeNode IdType { get; set; }

        public DispatchExplicitNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
