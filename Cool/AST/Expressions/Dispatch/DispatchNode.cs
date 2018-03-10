using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class DispatchNode : ExpressionNode
    {
        public abstract IdNode IdMethod { get; }

        public abstract List<ExpressionNode> Arguments { get; }

        public DispatchNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
