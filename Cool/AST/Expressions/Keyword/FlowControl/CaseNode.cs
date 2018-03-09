using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class CaseNode : KeywordNode
    {
        public ExpressionNode ExpressionCase => Children[0] as ExpressionNode;

        public List<(IdentifierNode Id, IdentifierNode TypeId, ExpressionNode Expression)> Branches { get; set; }

        public CaseNode(ParserRuleContext context) : base(context)
        {
            Branches = new List<(IdentifierNode Id, IdentifierNode TypeId, ExpressionNode Expression)>();
        }

    }
}
