using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class IfNode : KeywordNode
    {
        public ExpressionNode Condition { get; set; }
        public ExpressionNode Body { get; set; }
        public ExpressionNode ElseBody { get; set; }

        public IfNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            string repr = $"If Node (Line: {Line}, Column: {Column}),\n";
            repr += "Condition:\n| ";
            repr += $"{Condition}\n";
            repr += "Then:\n| ";
            repr += $"{Body}\n";
            repr += "Else:\n| ";
            repr += $"{ElseBody}\n";
            return repr.Replace("\n","\n| ");
        }

    }
}
