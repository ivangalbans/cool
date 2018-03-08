using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class BlockNode : ExpressionNode
    {
        public List<ExpressionNode> Expressions => Children.Select(x => x as ExpressionNode).ToList();

        public BlockNode(ParserRuleContext context) : base(context) { }

    }
}
