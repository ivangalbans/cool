using Antlr4.Runtime;
using System.Collections.Generic;
using System.Linq;

namespace Cool.AST
{
    class CaseNode : KeywordNode
    {
        public ExpressionNode ExpressionCase { get; set; }
        public List<(FormalNode, ExpressionNode)> Branches { get; set; }

        public CaseNode(ParserRuleContext context) : base(context)
        {
            Branches = new List<(FormalNode, ExpressionNode)>();
        }

    }
}
