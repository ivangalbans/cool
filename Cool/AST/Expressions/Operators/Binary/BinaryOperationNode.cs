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

        public override string ToString()
        {
            string[] name = base.ToString().Split('.');
            string repr = name[name.Length - 1] + $" (Line: {Line}, Column: {Column})\n";
            //repr += "Left:\n| ";
            repr += $"{LeftOperand}\n";
            //repr += $"Operation:{Symbol} \n";
            repr += $"{Symbol} \n";
            //repr += "Right:\n| ";
            repr += $"{RightOperand}\n";
            return repr.Replace("\n", "\n| ");
        }

    }
}
