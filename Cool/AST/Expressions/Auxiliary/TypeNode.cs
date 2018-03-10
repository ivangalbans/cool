
using Antlr4.Runtime;

namespace Cool.AST
{
    class TypeNode : AuxiliaryNode
    {
        public string TypeId { get; set; }

        public TypeNode(ParserRuleContext context, string typeId) : base(context)
        {
            TypeId = typeId;
        }

        public TypeNode(int line, int column, string typeId) : base(line, column)
        {
            TypeId = typeId;
        }

        private static NullType nullType = new NullType();

        public static NullType NULL => nullType;

    }
}
