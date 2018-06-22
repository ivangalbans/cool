using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class BoolNode : AtomNode
    {
        public bool Value { get; set; }

        public BoolNode(ParserRuleContext context, string text) : base(context)
        {
            Value = bool.Parse(text);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        //public override string ToString()
        //{
        //    return Value.ToString();
        //}
    }
}
