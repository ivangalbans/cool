using Antlr4.Runtime;

namespace Cool.AST
{
    public class IdNode : AuxiliaryNode
    {
        public string Text { get; set; }

        public IdNode(ParserRuleContext context, string text) : base(context)
        {
            Text = text;
        }

        public IdNode(int line, int column, string text) : base(line, column)
        {
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }

        #region NULL
        static readonly NullId nullId = new NullId();

        public static NullId NULL => nullId;

        public class NullId : IdNode
        {
            public NullId(int line = 0, int column = 0, string name = "Null-Id-Object") : base(line, column, name) { }
        }
        #endregion

    }
}
