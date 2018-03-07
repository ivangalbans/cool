using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    class AttributeNode : FeatureNode
    {
        public ExpressionNode Expression { get; set; }

        public TypeInfo TypeVar { get; set; }

        public string TextType { get; set; }

        public int LineType { get; set; }

        public int ColumnType { get; set; }


        public AttributeNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
