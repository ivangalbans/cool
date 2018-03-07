using System.Collections.Generic;
using Antlr4.Runtime;

namespace Cool.AST
{
    class CaseNode : KeywordNode
    {
        public ExpressionNode Expression { get; set; }

        public List<(string TextID, string TextType, ExpressionNode Expression)> Body { get; set; }

        public CaseNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
