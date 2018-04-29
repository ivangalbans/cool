using System.Collections.Generic;
using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    public class WhileNode : KeywordNode
    {
        public ExpressionNode Condition { get; set; }
        public ExpressionNode Body { get; set; }

        public WhileNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            string repr = $"While Node (Line: {Line}, Column: {Column}),\n";
            repr += "Condition:\n| ";
            repr += $"{Condition}\n";
            repr += "Then:\n| ";
            repr += $"{Body}\n";
            return repr.Replace("\n", "\n| ");
        }
    }
}
