using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class DispatchNode : ExpressionNode
    {
        public abstract string TextID { get; set; }

        public abstract int LineID { get; set; }

        public abstract int ColumnID { get; set; }

        public abstract List<ExpressionNode> ExpressionParams { get; set; }

        public DispatchNode(ParserRuleContext context) : base(context)
        {

        }
    }
}
