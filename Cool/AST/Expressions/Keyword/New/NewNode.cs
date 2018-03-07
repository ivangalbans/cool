using Antlr4.Runtime;

namespace Cool.AST
{
    class NewNode : KeywordNode
    {
        public string TextType { get; set; }

        public NewNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
