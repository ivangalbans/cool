using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Program and Class
        public void Visit(ProgramNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Classes.ForEach(cclass => cclass.Accept(this, cclass.Scope, errors));
        }

        public void Visit(ClassNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.FeatureNodes.ForEach(feature => feature.Accept(this, scope, errors));
        }
        #endregion

        #region Feature
        public void Visit(AttributeNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.AssignExp.Accept(this, scope, errors);
            var typeAssignExp = node.AssignExp.StaticType;

            if (!scope.IsDefinedType(node.Formal.Type.Text, out TypeInfo typeDeclared))
                errors.Add(SemanticError.NotDeclaredType(node.Formal.Type));

            if (!(typeAssignExp <= typeDeclared))
                errors.Add(SemanticError.CannotConvert(node.Formal.Type, typeAssignExp, typeDeclared));

            scope.Define(node.Formal.Id.Text, typeDeclared);
        }

        public void Visit(MethodNode node, IScope scope, ICollection<SemanticError> errors)
        {
            var scopeMethod = scope.CreateChild();
            foreach (var arg in node.Arguments)
            {
                if (!scope.IsDefinedType(arg.Type.Text, out TypeInfo typeArg))
                    errors.Add(SemanticError.NotDeclaredType(arg.Type));
                scopeMethod.Define(arg.Id.Text, typeArg);
            }
            
            if(!scope.IsDefinedType(node.TypeReturn.Text, out TypeInfo typeReturn))
                errors.Add(SemanticError.NotDeclaredType(node.TypeReturn));

            scope.Define(node.Id.Text, node.Arguments.Select(x => scope.GetType(x.Type.Text)).ToArray(), typeReturn);

            node.Body.Accept(this, scopeMethod, errors);

            if (!(node.Body.StaticType <= typeReturn))
                errors.Add(SemanticError.CannotConvert(node.Body, node.Body.StaticType, typeReturn));
            
            node.TypeReturn = new TypeNode(node.Body.Line, node.Body.Column, node.Body.StaticType.Text);
        }
        #endregion

        #region Unary Operation
        public void Visit(IsVoidNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Operand.Accept(this, scope, errors);

            if (!scope.IsDefinedType("Bool", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
        }

        public void Visit(NotNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Operand.Accept(this, scope, errors);

            if (node.Operand.StaticType.Text != "Bool")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.Operand.StaticType));

            if (!scope.IsDefinedType("Bool", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
        }

        public void Visit(NegNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Operand.Accept(this, scope, errors);

            if (node.Operand.StaticType.Text != "Int")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.Operand.StaticType));

            if (!scope.IsDefinedType("Int", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
        }
        #endregion

        #region Binary Operation
        public void Visit(ArithmeticOperation node, IScope scope, ICollection<SemanticError> errors)
        {
            node.LeftOperand.Accept(this, scope, errors);
            node.RightOperand.Accept(this, scope, errors);

            if (node.LeftOperand.StaticType.Text != "Int" || node.RightOperand.StaticType.Text != "Int")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.LeftOperand.StaticType, node.RightOperand.StaticType));

            if(!scope.IsDefinedType("Int", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
        }

        public void Visit(ComparisonOperation node, IScope scope, ICollection<SemanticError> errors)
        {
            node.LeftOperand.Accept(this, scope, errors);
            node.RightOperand.Accept(this, scope, errors);

            if (node.LeftOperand.StaticType.Text != "Int" || node.RightOperand.StaticType.Text != "Int")
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.LeftOperand.StaticType, node.RightOperand.StaticType));

            if (!scope.IsDefinedType("Bool", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
        }

        public void Visit(EqualNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.LeftOperand.Accept(this, scope, errors);
            node.RightOperand.Accept(this, scope, errors);

            if (node.LeftOperand.StaticType.Text != node.RightOperand.StaticType.Text || !(new string[3] { "Bool", "Int", "String"}.Contains(node.LeftOperand.StaticType.Text)))
                errors.Add(SemanticError.InvalidUseOfOperator(node, node.LeftOperand.StaticType, node.RightOperand.StaticType));

            if (!scope.IsDefinedType("Bool", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Bool")));
        }
        #endregion

        #region Block and Assignment
        public void Visit(BlockNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.ExpressionsBlock.ForEach(exp => exp.Accept(this, scope, errors));

            var last = node.ExpressionsBlock[node.ExpressionsBlock.Count - 1];

            if (!scope.IsDefinedType(last.StaticType.Text, out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(last.Line, last.Column, last.StaticType.Text)));
        }

        public void Visit(AssignmentNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.ExpressionRight.Accept(this, scope, errors);

            if (!scope.IsDefined(node.ID.Text, out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredVariable(node.ID));

            if (!(node.ExpressionRight.StaticType <= type))
                errors.Add(SemanticError.CannotConvert(node, node.ExpressionRight.StaticType, type));

            node.StaticType = node.ExpressionRight.StaticType;
        }
        #endregion

        #region Dispatch
        public void Visit(DispatchExplicitNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Expression.Accept(this, scope, errors);

            if (!scope.IsDefinedType(node.IdType.Text, out TypeInfo typeSuperClass))
                errors.Add(SemanticError.NotDeclaredType(node.IdType));

            if (!(node.Expression.StaticType <= typeSuperClass))
                errors.Add(SemanticError.CannotConvert(node, node.Expression.StaticType, typeSuperClass));

            node.Arguments.ForEach(x => x.Accept(this, scope, errors));

            var scopeSuperClass = typeSuperClass.ClassReference.Scope;
            if (!(scopeSuperClass.IsDefined(node.IdMethod.Text, node.Arguments.Select(x => x.StaticType).ToArray(), out node.StaticType)))
                errors.Add(SemanticError.NotDeclareFunction(node, node.IdMethod.Text));
        }

        public void Visit(DispatchImplicitNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Arguments.ForEach(expArg => expArg.Accept(this, scope, errors));

            if (!scope.IsDefined(node.IdMethod.Text, node.Arguments.Select(x => x.StaticType).ToArray(), out node.StaticType))
                errors.Add(SemanticError.NotDeclareFunction(node, node.IdMethod.Text));
        }
        #endregion

        #region Atom
        public void Visit(IntNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if(!scope.IsDefinedType("Int", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
        }

        public void Visit(BoolNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType("Bool", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
        }

        public void Visit(StringNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType("String", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Int")));
        }
        #endregion

        #region Identifier and Formal
        public void Visit(IdentifierNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefined(node.Text, out node.StaticType))
                errors.Add(SemanticError.NotDeclaredVariable(node));
        }

        public void Visit(FormalNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType(node.Type.Text, out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(node.Type));
        }
        #endregion

        #region Keywords
        public void Visit(CaseNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Condition.Accept(this, scope, errors);
            node.Body.Accept(this, scope, errors);
            node.ElseBody.Accept(this, scope, errors);

            if (node.Condition.StaticType.Text != "Bool")
                errors.Add(SemanticError.CannotConvert(node.Condition, node.Condition.StaticType, scope.GetType("Bool")));

            node.StaticType = Algorithm.LowerCommonAncestor(node.Body.StaticType, node.ElseBody.StaticType);
        }

        public void Visit(LetNode node, IScope scope, ICollection<SemanticError> errors)
        {
            var scopeLet = scope.CreateChild();

            foreach (var expInit in node.Initialization)
            {
                expInit.AssignExp.Accept(this, scopeLet, errors);
                var typeAssignExp = expInit.AssignExp.StaticType;

                if (!scopeLet.IsDefinedType(expInit.Formal.Type.Text, out TypeInfo typeDeclared))
                    errors.Add(SemanticError.NotDeclaredType(expInit.Formal.Type));

                if (!(typeAssignExp <= typeDeclared))
                    errors.Add(SemanticError.CannotConvert(expInit.Formal.Type, typeAssignExp, typeDeclared));

                if (scopeLet.IsDefined(expInit.Formal.Id.Text, out TypeInfo typeOld))
                    scopeLet.Change(expInit.Formal.Id.Text, typeDeclared);
                else
                    scopeLet.Define(expInit.Formal.Id.Text, typeDeclared);
            }

            /*
            node.AssignExp.Accept(this, scope, errors);
            var typeAssignExp = node.AssignExp.StaticType;

            if (!scope.IsDefinedType(node.Formal.Type.Text, out TypeInfo typeDeclared))
                errors.Add(SemanticError.NotDeclaredType(node.Formal.Type));

            if (!(typeAssignExp <= typeDeclared))
                errors.Add(SemanticError.CannotConvert(node.Formal.Type, typeAssignExp, typeDeclared));

            scope.Define(node.Formal.Id.Text, typeDeclared);
            */






            node.ExpressionBody.Accept(this, scopeLet, errors);
            node.StaticType = node.ExpressionBody.StaticType;

        }

        public void Visit(NewNode node, IScope scope, ICollection<SemanticError> errors)
        {
            if (!scope.IsDefinedType(node.TypeId.Text, out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(node.TypeId));
        }

        public void Visit(WhileNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Condition.Accept(this, scope, errors);
            node.Body.Accept(this, scope, errors);

            if (node.Condition.StaticType.Text != "Bool")
                errors.Add(SemanticError.CannotConvert(node.Condition, node.Condition.StaticType, scope.GetType("Bool")));

            if (!scope.IsDefinedType("Object", out node.StaticType))
                errors.Add(SemanticError.NotDeclaredType(new TypeNode(node.Line, node.Column, "Object")));
        }
        #endregion

    }
}
