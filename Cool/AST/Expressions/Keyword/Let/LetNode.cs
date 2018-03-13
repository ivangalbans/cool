using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class LetNode : KeywordNode
    {
        public List<AttributeNode> Initialization { get; set; }
        public ExpressionNode ExpressionBody { get; set; }

        public LetNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
