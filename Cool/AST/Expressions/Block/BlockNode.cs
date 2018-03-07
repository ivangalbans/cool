using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class BlockNode : ExpressionNode
    {
        public List<ExpressionNode> Expressions { get; set; }

        public BlockNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
