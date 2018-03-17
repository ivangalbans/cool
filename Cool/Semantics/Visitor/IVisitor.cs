using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cool.AST;

namespace Cool.Semantics
{
    public interface IVisitor
    {
        void Visit(AssignmentNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(AttributeNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(BinaryOperationNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(BlockNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(BoolNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(CaseNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(ClassNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(DispatchExplicitNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(DispatchImplicitNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(EqualNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(ComparisonOperation node, IScope scope, ICollection<SemanticError> errors);
        void Visit(IfNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(IntNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(IsVoidNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(LetNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(MethodNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(NegNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(NewNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(NotNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(ProgramNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(StringNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(IdentifierNode node, IScope scope, ICollection<SemanticError> errors);
        void Visit(WhileNode node, IScope scope, ICollection<SemanticError> errors);
    }
}
