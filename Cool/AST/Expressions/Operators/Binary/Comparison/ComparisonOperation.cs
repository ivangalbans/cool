using Antlr4.Runtime;

namespace Cool.AST
{
    abstract class ComparisonOperation : BinaryOperationNode
    {
        public ComparisonOperation(ParserRuleContext context) : base(context) { }
    }
}
