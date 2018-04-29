using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;

namespace Cool.AST
{
    public abstract class DispatchNode : ExpressionNode
    {
        public IdNode IdMethod { get; set; }

        public List<ExpressionNode> Arguments { get; set; }

        public DispatchNode(ParserRuleContext context) : base(context)
        {

        }

        public override string ToString()
        {
            string repr = $"{IdMethod}(\n";
            foreach (var a in Arguments)
            {
                repr += $"{a}\n";
            }
            repr += ")\n";
            return repr.Replace("\n","\n| ");
        }

    }
}
