using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cool.AST;

namespace Cool.Semantics
{

    public class Tour1 : IVisitor, ICheckSemantics
    {
        public ProgramNode CheckSemantic(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Accept(this, scope, errors);
            return node;
        }

        public void Visit(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!Algorithm.TopologicalSort(node.Classes, errors))
                return;
            foreach (var cclass in node.Classes)
                scope.AddType(cclass.TypeClass.Text, new TypeInfo(cclass.TypeClass.Text, scope.GetType(cclass.TypeInherit.Text), cclass));
            foreach (var item in node.Classes)
                item.Accept(this, scope, errors);
        }

        public void Visit(ClassNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Scope = new Scope
            {
                Type = scope.GetType(node.TypeClass.Text),
                Parent = scope.GetType(node.TypeInherit.Text).ClassReference.Scope
            };

            //scope.Parent = scope.GetType(node.TypeInherit.Text).ClassReference.Scope;
            foreach (var item in node.FeatureNodes)
                item.Accept(this, node.Scope, errors);
        }

        public void Visit(AttributeNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType(node.Formal.Type.Text, out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(node.Formal.Type));
            if (scope.IsDefined(node.Formal.Id.Text, out TypeInfo t))
                errors.Add(SemanticError.RepeatedVariable(node.Formal.Id));
            scope.Define(node.Formal.Id.Text, type);
        }

        public void Visit(MethodNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType(node.TypeReturn.Text, out TypeInfo typeReturn))
                errors.Add(SemanticError.NotDeclaredType(node.TypeReturn));

            node.TypeReturn = new TypeNode(node.TypeReturn.Line, node.TypeReturn.Column, typeReturn.Text);

            TypeInfo[] typeArgs = new TypeInfo[node.Arguments.Count];
            foreach (var arg in node.Arguments)
                if (!scope.IsDefinedType(arg.Type.Text, out TypeInfo typeParam))
                    errors.Add(SemanticError.NotDeclaredType(arg.Type));

            scope.Define(node.Id.Text, typeArgs, typeReturn);
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
