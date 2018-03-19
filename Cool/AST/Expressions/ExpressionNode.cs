using System.Collections.Generic;
using Antlr4.Runtime;

using Cool.Semantics;

namespace Cool.AST
{
    public abstract class ExpressionNode : ASTNode, IVisit
    {
        public TypeInfo StaticType = TypeInfo.NULL;

        public ExpressionNode(ParserRuleContext context) : base(context) { }

        public ExpressionNode(int line, int column) : base(line, column) { }

        public abstract void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors);

        #region NULL
        private static NullExpression nullExpression = new NullExpression();

        public static NullExpression NULL => nullExpression;

        public class NullExpression : ExpressionNode
        {
            public NullExpression(int line = 0, int column = 0) : base(line, column) { }

            public override void Accept(IVisitor visitor, IScope scope, ICollection<SemanticError> errors)
            {
            }
        }
        #endregion

    }
}
