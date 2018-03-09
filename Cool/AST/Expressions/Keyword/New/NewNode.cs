using Antlr4.Runtime;

namespace Cool.AST
{
    class NewNode : KeywordNode
    {
        public IdNode TypeId => Children[0] as IdNode;

        public NewNode(ParserRuleContext context) : base(context) { }

    }
}
