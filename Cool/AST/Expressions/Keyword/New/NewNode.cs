using Antlr4.Runtime;

namespace Cool.AST
{
    class NewNode : KeywordNode
    {
        public TypeNode TypeId => Children[0] as TypeNode;

        public NewNode(ParserRuleContext context) : base(context) { }

    }
}
