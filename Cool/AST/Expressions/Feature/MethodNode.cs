using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    public class MethodNode : FeatureNode
    {
        public IdNode Id { get; set; }
        public List<FormalNode> Arguments { get; set; }
        public TypeNode TypeReturn { get; set; }
        public ExpressionNode Body { get; set; }

        public MethodNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
