using System.Collections.Generic;
using Antlr4.Runtime;

using Cool.Semantics;

namespace Cool.AST
{
    public abstract class ExpressionNode : ASTNode, IVisit
    {
        public TypeInfo StaticType = TypeInfo.OBJECT;

        public ExpressionNode(ParserRuleContext context) : base(context) { }

        public ExpressionNode(int line, int column) : base(line, column) { }

        public abstract void Accept(IVisitor visitor, IScope scope, ICollection<string> errors);

        #region VOID

        public class VoidExpression : ExpressionNode
        {
            public string GetStaticType { get; }
            public VoidExpression(string type, int line = 0, int column = 0) : base(line, column)
            {
                GetStaticType = type;
            }

            public override void Accept(IVisitor visitor, IScope scope, ICollection<string> errors)
            {
                visitor.Visit(this, scope, errors);
            }
        }
        #endregion

    }
}
