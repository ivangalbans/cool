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

            node.Classes.ForEach(cclass => scope.AddType(cclass.TypeClass.Text, new TypeInfo(cclass.TypeClass.Text, scope.GetType(cclass.TypeInherit.Text), cclass)));

            int idMain = -1;
            for (int i = 0; i < node.Classes.Count; ++i)
                if (node.Classes[i].TypeClass.Text == "Main")
                    idMain = i;

            if (idMain == -1)
            {
                errors.Add(SemanticError.NotFoundClassMain());
                return;
            }

            bool mainOK = false;
            foreach (var item in node.Classes[idMain].FeatureNodes)
            {
                if(item is MethodNode)
                {
                    var method = item as MethodNode;
                    if (method.Id.Text == "main" && method.Arguments.Count == 0)
                        mainOK = true;
                }
            }

            if (!mainOK)
                errors.Add(SemanticError.NotFoundMethodmain(node.Classes[idMain]));

            foreach (var cclass in node.Classes)
            {
                if (!scope.IsDefinedType(cclass.TypeInherit.Text, out TypeInfo type))
                {
                    errors.Add(SemanticError.NotDeclaredType(cclass.TypeInherit));
                    return;
                }
                cclass.Accept(this, scope, errors);
            }
        }

        public void Visit(ClassNode node, IScope scope, ICollection<SemanticError> errors)
        {
            node.Scope = new Scope
            {
                Type = scope.GetType(node.TypeClass.Text),
                Parent = scope.GetType(node.TypeInherit.Text).ClassReference.Scope
            };
            node.FeatureNodes.ForEach(feature => feature.Accept(this, node.Scope, errors));
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
            for(int i = 0; i < node.Arguments.Count; ++i)
                if (!scope.IsDefinedType(node.Arguments[i].Type.Text, out typeArgs[i]))
                    errors.Add(SemanticError.NotDeclaredType(node.Arguments[i].Type));

            scope.Define(node.Id.Text, typeArgs, typeReturn);
        }

        #region NOT IMPLEMENTATION
        public void Visit(AssignmentNode node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }

        public void Visit(ArithmeticOperation node, IScope scope, ICollection<SemanticError> errors)
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

        public void Visit(FormalNode formalNode, IScope scope, ICollection<SemanticError> errors)
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

        public void Visit(ExpressionNode.VoidExpression node, IScope scope, ICollection<SemanticError> errors)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
