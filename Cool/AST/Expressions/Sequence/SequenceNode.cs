using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class SequenceNode : ExpressionNode
    {
        public List<ExpressionNode> Sequence { get; set; }

        public SequenceNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        //public override string ToString()
        //{
        //    string repr = "";
        //    foreach (var e in Sequence)
        //    {
        //        repr += $"{e}\n";
        //    }
        //    return repr;
        //}
    }
}
