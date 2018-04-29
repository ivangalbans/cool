using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class NewNode : KeywordNode
    {
        public TypeNode TypeId { get; set; }

        public NewNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }
        public override string ToString()
        {
            string repr = $"New Node (Line: {Line}, Column: {Column}) {TypeId}";
            return repr;
        }
    }
}
