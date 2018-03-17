using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cool.AST;

namespace Cool.Semantics
{

    class Tour1 : IVisitor, ICheckSemantics
    {
        public ProgramNode CheckSemantic(ProgramNode node, ICollection<SemanticError> errors)
        {
            node.Accept(this, null, errors);
            return node;
        }

        public void Visit(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            Algorithm.TopologicalSort(node.Classes, errors);
            foreach (var item in node.Classes)
                item.Accept(this, scope, errors);
        }

        public void Visit(ClassNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Scope = new Scope
            {
                Type = Scope.DeclaredTypes[node.TypeClass.TypeId],
                Parent = Scope.DeclaredTypes[node.TypeInherit.TypeId].ClassReference.Scope
            };
            foreach (var item in node.FeatureNodes)
                item.Accept(this, node.Scope, errors);
        }

        public void Visit(AttributeNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType(node.Formal.Id.Name, out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(node.Formal.TypeId));
            if (scope.IsDefined(node.Formal.Id.Name, out TypeInfo t))
                errors.Add(SemanticError.RepeatedVariable(node.Formal.Id));
            scope.Define(node.Formal.Id.Name, type);
        }

        public void Visit(MethodNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType(node.TypeReturn.TypeId, out TypeInfo typeReturn))
                errors.Add(SemanticError.NotDeclaredType(node.TypeReturn));

            //node.TypeReturn = new TypeNode(node.TypeReturn.)
        }

        #region NOT IMPLEMENTATION
        public void Visit(AssignmentNode node, IScope scope, ICollection<SemanticError> errors)
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
        #endregion

    }

}
