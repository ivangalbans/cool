using Antlr4.Runtime;
using Cool.Semantics;
using System.Linq;

namespace Cool.AST
{
    class EqualNode : ComparisonOperation
    {
        public EqualNode(ParserRuleContext context) : base(context)
        {
        }
    }
}