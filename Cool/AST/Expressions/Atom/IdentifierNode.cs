
using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class IdentifierNode : AtomNode
    {
        public string Text { get; set; }

        public IdentifierNode(ParserRuleContext context, string text) : base(context)
        {
            Text = text;
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        //public override string ToString()
        //{
        //    return Text;
        //}
    }
}
