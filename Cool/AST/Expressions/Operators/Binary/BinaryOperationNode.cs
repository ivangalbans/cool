using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public abstract class BinaryOperationNode : ExpressionNode
    {
        public ExpressionNode LeftOperand { get; set; }
        public ExpressionNode RightOperand { get; set; }

        public BinaryOperationNode(ParserRuleContext context) : base(context)
        {
        }

        public abstract string Symbol { get; }

    }
}
