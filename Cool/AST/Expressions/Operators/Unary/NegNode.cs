using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class NegNode : UnaryOperationNode
    {

        public NegNode(ParserRuleContext context) : base(context)
        {
            Type = Types.Int;
        }

    }
}
