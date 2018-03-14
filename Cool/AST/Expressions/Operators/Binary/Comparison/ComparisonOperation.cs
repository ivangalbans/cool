using Antlr4.Runtime;
using Cool.Semantics;
using System.Collections.Generic;

namespace Cool.AST
{
    public abstract class ComparisonOperation : BinaryOperationNode
    {
        public ComparisonOperation(ParserRuleContext context) : base(context)
        {
        }

    }
}
