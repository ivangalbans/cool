using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Cool.AST
{
    class MethodCallNode : ExpressionNode
    {
        public IdNode Identifier => Children[0] as IdNode;

        public List<ExpressionNode> Arguments => Children.Skip(1).Cast<ExpressionNode>().ToList();

        public MethodCallNode(ParserRuleContext context) : base(context) { }

    }
}
