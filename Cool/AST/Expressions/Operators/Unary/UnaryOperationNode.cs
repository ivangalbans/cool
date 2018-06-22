using Antlr4.Runtime;

namespace Cool.AST
{
    public abstract class UnaryOperationNode : ExpressionNode
    {
        public ExpressionNode Operand { get; set; }

        public abstract string Symbol { get; }

        public UnaryOperationNode(ParserRuleContext context) : base(context)
        {
        }

        //public override string ToString()
        //{
        //    string[] name = base.ToString().Split('.');
        //    string repr = name[name.Length - 1] + $" (Line: {Line}, Column: {Column})\n";
        //    repr += $"{Symbol} \n";
        //    repr += $"{Operand}\n";
        //    return repr.Replace("\n", "\n| ");
        //}
    }
}
