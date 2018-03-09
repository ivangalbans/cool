using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class BlockNode : ExpressionNode
    {
        public List<ExpressionNode> Expressions { get; set; }

        public BlockNode(ParserRuleContext context) : base(context)
        {
            Expressions = new List<ExpressionNode>();
        }
    }
}
