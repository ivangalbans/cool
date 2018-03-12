using Antlr4.Runtime;
using Cool.Semantics;
using System.Linq;

namespace Cool.AST
{
    class Less : ComparisonOperation
    {
        public Less(ParserRuleContext context) : base(context)
        {
        }
    }
}
