using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class AttributeNode : FeatureNode
    {
        public FormalNode Formal => Children[0] as FormalNode;
        public ExpressionNode Expression => Children[1] as ExpressionNode;

        public AttributeNode(ParserRuleContext context) : base(context) { }

    }
}
