using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;

namespace Cool.AST
{
    class DispatchImplicitNode : DispatchNode
    {
        public IdentifierNode IdMethod => Children[0] as IdentifierNode;

        public List<ExpressionNode> Arguments => Children.Skip(1).Cast<ExpressionNode>().ToList();

        public DispatchImplicitNode(ParserRuleContext context) : base(context) { }

    }
}
