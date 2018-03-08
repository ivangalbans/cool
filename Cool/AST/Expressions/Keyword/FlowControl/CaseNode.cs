using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class CaseNode : KeywordNode
    {
        public ExpressionNode Expression => Children[0] as ExpressionNode;

        public List<(IdentifierNode ID, IdentifierNode Type, ExpressionNode Expression)> Branches { get; set; }

        public CaseNode(ParserRuleContext context) : base(context) { }

    }
}
