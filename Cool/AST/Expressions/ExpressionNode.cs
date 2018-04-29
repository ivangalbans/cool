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

        public abstract void Accept(IVisitor visitor);

    }
}
