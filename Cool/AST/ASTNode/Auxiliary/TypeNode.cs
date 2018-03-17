
using System.Collections.Generic;
using Antlr4.Runtime;
using Cool.Semantics;

namespace Cool.AST
{
    public class TypeNode : AuxiliaryNode
    {
        public string Text { get; set; }

        public TypeNode(ParserRuleContext context, string text) : base(context)
        {
            Text = text;
        }

        public TypeNode(int line, int column, string text) : base(line, column)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
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
