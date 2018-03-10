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

        protected override bool SupportType(TypeInfo type)
        {
            return new[] { Types.Int, Types.String }.Contains(type);
        }
    }
}