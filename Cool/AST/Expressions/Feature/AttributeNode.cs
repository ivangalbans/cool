using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class AttributeNode : FeatureNode
    {
        public FormalNode Formal { get; set; }
        public ExpressionNode AssignExp { get; set; }

        public AttributeNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
