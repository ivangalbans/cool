using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using AST.Scope;
using AST.Visitor;
using Grammars;

namespace AST.Nodes
{
    public class LetNode : KeywordNode
    {
        public List<(string TextID, string TextType, ExpressionNode ExpressionInit)> ExpressionInitialization;

        public List<((int LineID, int ColumnID), (int LineType, int ColumnType))> LineColumnExpressionInitialization;

        public ExpressionNode ExpressionBody { get; set; }

        public LetNode(List<(Token ID, Token Type, ExpressionNode ExpressionInit)> expressionInitialization, ExpressionNode expression)
        {
            ExpressionInitialization = expressionInitialization.Select(x => (x.ID.Text, x.Type.Text, x.ExpressionInit)).ToList();
            LineColumnExpressionInitialization = expressionInitialization.Select(x => ((x.ID.Line, x.ID.Column), (x.Type.Line, x.Type.Column))).ToList();
            ExpressionBody = expression;
        }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}
