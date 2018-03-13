using Antlr4.Runtime;

namespace Cool.AST
{
    class NewNode : KeywordNode
    {
        public TypeNode TypeId { get; set; }

        public NewNode(ParserRuleContext context) : base(context)
        {
        }

    }
}
