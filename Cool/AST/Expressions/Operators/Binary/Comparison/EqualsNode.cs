using Antlr4.Runtime;

namespace Cool.AST
{
    class EqualNode : ComparisonOperation
    {
        public EqualNode(ParserRuleContext context) : base(context) { }
        
    }
}