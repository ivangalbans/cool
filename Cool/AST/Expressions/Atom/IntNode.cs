using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class IntNode : AtomNode
    {
        public int Value { get; set; }

        public IntNode(ParserRuleContext context, string text) : base(context)
        {
            Value = int.Parse(text);
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
