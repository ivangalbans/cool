using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class DispatchExplicitNode : DispatchNode
    {
        public ExpressionNode Expression => Children[0] as ExpressionNode;
        public TypeNode IdType => Children[1] as TypeNode;
        public override IdNode IdMethod => Children[2] as IdNode;
        public override List<ExpressionNode> Arguments => Children.Skip(3).Cast<ExpressionNode>().ToList();

        public DispatchExplicitNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
