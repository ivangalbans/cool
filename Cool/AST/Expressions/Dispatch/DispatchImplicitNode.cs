using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.AST
{
    class DispatchImplicitNode : DispatchNode
    {
        public override IdNode IdMethod => Children[0] as IdNode;

        public override List<ExpressionNode> Arguments => Children.Skip(1).Cast<ExpressionNode>().ToList();

        public DispatchImplicitNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
