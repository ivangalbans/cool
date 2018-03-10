using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class LetNode : KeywordNode
    {
        public List<AttributeNode> Initialization => Children.GetRange(0, Children.Count - 1).Cast<AttributeNode>().ToList();
        public ExpressionNode ExpressionBody => Children[Children.Count - 1] as ExpressionNode;

        public LetNode(ParserRuleContext context) : base(context) { }

    }
}
