using Antlr4.Runtime;

namespace Cool.AST
{
    class IdNode : AuxiliaryNode
    {
        public string Name { get; set; }

        public IdNode(ParserRuleContext context, string name) : base(context)
        {
            Name = name;
        }

        public IdNode(int line, int column, string name) : base(line, column)
        {
            Name = name;
        }

        #region NULL
        static readonly NullId nullId = new NullId();

        public new static NullId NULL => nullId;

        internal class NullId : IdNode
        {
            public NullId(int line = 0, int column = 0, string name = "Null-Id-Object") : base(line, column, name) { }
        }
        #endregion

    }
}
