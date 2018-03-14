﻿using Antlr4.Runtime;

namespace Cool.AST
{
    public abstract class UnaryOperationNode : ExpressionNode
    {
        public ExpressionNode Operand { get; set; }

        public abstract string OperatorName { get; }

        public UnaryOperationNode(ParserRuleContext context) : base(context)
        {
        }
    }
}
