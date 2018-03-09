using Antlr4.Runtime;
using System.Collections.Generic;

namespace Cool.AST
{
    class LetNode : KeywordNode
    {
        public List<(IdentifierNode Id, IdentifierNode TypeId, ExpressionNode Expression)> ExpressionsInitials { get; set; }

        public ExpressionNode ExpressionBody { get; set; }

        public LetNode(ParserRuleContext context) : base(context)
        {
            ExpressionsInitials = new List<(IdentifierNode Id, IdentifierNode TypeId, ExpressionNode Expression)>();
        }

    }
}
