using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class LetNode : KeywordNode
    {
        public List<AttributeNode> Initialization { get; set; }
        public ExpressionNode ExpressionBody { get; set; }

        public LetNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            string repr = $"Let Node (Line: {Line}, Column: {Column}),\n";
            repr += "Initialization:\n| ";
            foreach (var a in Initialization)
            {
                repr += $"{a}\n";
            }
            repr += $"Body:\n| ";
            repr += $"{ExpressionBody}\n";
            return repr.Replace("\n","\n| ");
        }

    }
}
