using Antlr4.Runtime;

namespace Cool.AST
{
    public abstract class ExpressionNode : ASTNode
    {
        public ExpressionNode(ParserRuleContext context) : base(context) { }

        public ExpressionNode(int line, int column) : base(line, column) { }

        #region NULL
        private static NullExpression nullExpression = new NullExpression();

        public static NullExpression NULL => nullExpression;

        public class NullExpression : ExpressionNode
        {
            public NullExpression(int line = 0, int column = 0) : base(line, column) { }
        }
        #endregion

    }
}
