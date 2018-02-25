using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes;
using AST.Nodes.Abstract;
using AST.Scope;

namespace AST.Visitor
{
    public interface IVisitor
    {

        void Visit(ProgramNode node, IScope scope);
        void Visit(ClassNode node, IScope scope);
        void Visit(AssignmentNode node, IScope scope);

        #region BinaryOperationNode
        void Visit(ComparisonOperation node, IScope scope);
        void Visit(ArithmeticOperation node, IScope scope);
        #endregion

        #region AtomNode
        void Visit(IntNode node, IScope scope);
        void Visit(StringNode node, IScope scope);
        void Visit(VoidNode node, IScope scope);
        void Visit(BoolNode node, IScope scope);
        void Visit(IdentifierNode node, IScope scope);
        #endregion

        #region DispatchNode
        void Visit(DispatchExplicitNode node, IScope scope);
        void Visit(DispatchImplicitNode node, IScope scope);
        #endregion

        #region UnaryOperationNode
        void Visit(NegNode node, IScope scope);
        void Visit(NotNode node, IScope scope);
        void Visit(IsVoidNode node, IScope scope);
        #endregion

        #region KeywordNode
        void Visit(CaseNode node, IScope scope);
        void Visit(IfNode node, IScope scope);
        void Visit(WhileNode node, IScope scope);
        void Visit(LetNode node, IScope scope);
        void Visit(NewNode node, IScope scope);
        #endregion

        #region FeatureNode
        void Visit(AttributeNode node, IScope scope);
        void Visit(MethodNode node, IScope scope);
        #endregion



    }
}
