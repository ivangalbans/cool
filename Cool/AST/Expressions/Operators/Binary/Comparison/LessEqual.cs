using Antlr4.Runtime;
using Cool.Semantics;
using System.Linq;

namespace Cool.AST
{
    class LessEqual : ComparisonOperation
    {
        public LessEqual(ParserRuleContext context) : base(context)
        {
        }
    }
}
