
using System.Collections.Generic;
using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    public class TypeNode : AuxiliaryNode
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

        public override string ToString()
        {
            return TypeId;
        }

        #region NULL
        private static readonly NullType nullType = new NullType();

        public static NullType NULL => nullType;

        public class NullType : TypeNode
        {
            public NullType(int line = 0, int column = 0, string name = "Object") : base(line, column, name) { }
        }
        #endregion

    }
}
