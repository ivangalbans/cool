using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.AST;

namespace Cool.Semantics
{
    public class Tour2 : IVisitor, ICheckSemantics
    {
        public ProgramNode CheckSemantic(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Accept(this, scope, errors);
            return node;
        }

        public void Visit(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            foreach (var cclass in node.Classes)
                cclass.Accept(this, cclass.Scope, errors);
        }

        public void Visit(ClassNode node, IScope scope, ICollection<SemanticError> errors)
        {
            foreach (var feature in node.FeatureNodes)
                feature.Accept(this, node.Scope, errors);
        }

        public void Visit(AttributeNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.AssignExp.Accept(this, scope, errors);
            var typeAssignExp = node.AssignExp.Type;

            if (!scope.IsDefinedType(node.Formal.Type.Text, out TypeInfo typeDeclared))
                errors.Add(SemanticError.NotDeclaredType(node.Formal.Type));

            if (!(typeAssignExp <= typeDeclared))
                errors.Add(SemanticError.CannotConvert(node.Formal.Type, typeAssignExp, typeDeclared));

            scope.Define(node.Formal.Id.Text, typeDeclared);
        }

        public void Visit(MethodNode node, IScope scope, ICollection<SemanticError> errors)
        {
            
        }

        public void Visit(AssignmentNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        #region Unary Operation
        public void Visit(IsVoidNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Operand.Accept(this, scope, errors);

            if(!scope.IsDefinedType("Bool", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
            node.Type = type;
        }

        public void Visit(NotNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Operand.Accept(this, scope, errors);

            if (node.Operand.Type.Text != "Bool")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.Operand.Type));

            if (!scope.IsDefinedType("Bool", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
            node.Type = type;
        }

        public void Visit(NegNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Operand.Accept(this, scope, errors);

            if (node.Operand.Type.Text != "Int")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.Operand.Type));

            if (!scope.IsDefinedType("Int", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
            node.Type = type;
        }
        #endregion

        #region Binary Operation
        public void Visit(ArithmeticOperation node, IScope scope, ICollection<SemanticError> errors)
        {
            node.LeftOperand.Accept(this, scope, errors);
            node.RightOperand.Accept(this, scope, errors);

            if (node.LeftOperand.Type.Text != "Int" || node.RightOperand.Type.Text != "Int")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.LeftOperand.Type, node.RightOperand.Type));

            if(!scope.IsDefinedType("Int", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
            node.Type = type;
        }

        public void Visit(ComparisonOperation node, IScope scope, ICollection<SemanticError> errors)
        {
            node.LeftOperand.Accept(this, scope, errors);
            node.RightOperand.Accept(this, scope, errors);

            if (node.LeftOperand.Type.Text != "Int" || node.RightOperand.Type.Text != "Int")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.LeftOperand.Type, node.RightOperand.Type));

            if (!scope.IsDefinedType("Bool", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
            node.Type = type;
        }

        public void Visit(EqualNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.LeftOperand.Accept(this, scope, errors);
            node.RightOperand.Accept(this, scope, errors);

            if (node.LeftOperand.Type.Text != node.RightOperand.Type.Text || !(new string[3] { "Bool", "Int", "String"}.Contains(node.LeftOperand.Type.Text)))
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.LeftOperand.Type, node.RightOperand.Type));

            if (!scope.IsDefinedType("Bool", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
            node.Type = type;
        }
        #endregion

        public void Visit(BlockNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(CaseNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        #region Dispatch
        public void Visit(DispatchExplicitNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchImplicitNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }
        #endregion

        public void Visit(IfNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        #region Atom Node
        public void Visit(IntNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if(!scope.IsDefinedType("Int", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
            node.Type = type;
        }

        public void Visit(BoolNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType("Bool", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
            node.Type = type;
        }

        public void Visit(StringNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType("String", out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
            node.Type = type;
        }
        #endregion

        public void Visit(LetNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(NewNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        #region Auxiliary Node
        public void Visit(IdentifierNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefined(node.Text, out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredVariable(node));
            node.Type = type;
        }
        #endregion

        public void Visit(WhileNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }
    }
}
