using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public class AttributeNode : FeatureNode
    {
        public FormalNode Formal { get; set; }
        public ExpressionNode AssignExp { get; set; }

        public AttributeNode(ParserRuleContext context) : base(context)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
