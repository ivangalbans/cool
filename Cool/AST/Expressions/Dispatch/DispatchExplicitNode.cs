using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class DispatchExplicitNode : DispatchNode
    {
        public ExpressionNode Expression { get; set; }
        public TypeNode IdType { get; set; }

        public DispatchExplicitNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            string repr = $"Dispatch Explicit Node (Line: {Line}, Column: {Column})\n";
            repr += "Object:\n| ";
            repr += $"{Expression}\n";
            repr += "@Type:\n| ";
            repr += $"{IdType}\n";
            repr += "Dispatch:\n| ";
            repr += base.ToString();
            return repr.Replace("\n", "\n| ");
        }
    }
}
