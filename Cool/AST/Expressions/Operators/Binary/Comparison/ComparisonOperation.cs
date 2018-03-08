using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    abstract class ComparisonOperation : BinaryOperationNode
    {
        public ComparisonOperation(ParserRuleContext context) : base(context)
        {
            Type = Types.Int;
        }

        protected abstract bool SupportType(TypeInfo type);

    }
}
