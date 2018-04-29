using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class StringNode : AtomNode
    {
        public string Text { get; set; }

        public StringNode(ParserRuleContext context, string text) : base(context)
        {
            Text = text;
        }

        public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
        {
            visitor.Visit(this, scope, errors);
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
