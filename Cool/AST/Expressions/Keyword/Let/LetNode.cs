using Antlr4.Runtime;
using System.Collections.Generic;

namespace Cool.AST
{
    class LetNode : KeywordNode
    {
        public List<(string TextID, string TextType, ExpressionNode ExpressionInit)> ExpressionInitialization;

        public List<((int LineID, int ColumnID), (int LineType, int ColumnType))> LineColumnExpressionInitialization;

        public ExpressionNode ExpressionBody { get; set; }

        public LetNode(ParserRuleContext context) : base(context)
        {

        }

    }
}
