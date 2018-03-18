using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.AST;

namespace Cool.Semantics.TourAST
{
    public class Tour2 : IVisitor, ICheckSemantics
    {
        public ProgramNode CheckSemantic(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Accept(this, scope, errors);
            return node;
        }

        public void Visit(AssignmentNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(AttributeNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(BinaryOperationNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(BlockNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoolNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(CaseNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(ClassNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchExplicitNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchImplicitNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(EqualNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(ComparisonOperation node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(IsVoidNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(LetNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(MethodNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(NegNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(NewNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(NotNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(StringNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdentifierNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(WhileNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }
    }
}
