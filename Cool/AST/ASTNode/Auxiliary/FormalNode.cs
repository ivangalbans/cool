using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class FormalNode : ExpressionNode
    {
        public IdentifierNode Id { get; set; }
        public TypeNode Type { get; set; }

        public FormalNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            string repr = $"{Id} : {Type}";
            return repr;
        }
    }
}
