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
        IScope scope;
        ICollection<string> errors;
        public ProgramNode CheckSemantic(ProgramNode node, IScope scope, ICollection<string> errors)
        {
            this.scope = scope;
            this.errors = errors;
            node.Accept(this);
            return node;
        }

        public void Visit(ProgramNode node)
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
                if(new List<string>{ "Bool", "Int", "String"}.Contains(type.Text))
                {
                    errors.Add(SemanticError.NotInheritsOf(cclass, type));
                    return;
                }
                cclass.Accept(this);
            }
        }

        public void Visit(ClassNode node)
        {
            Tour1 tour = new Tour1();
            tour.scope = new Scope
            {
                Type = scope.GetType(node.TypeClass.Text),
                Parent = scope.GetType(node.TypeInherit.Text).ClassReference.Scope
            };
            tour.errors = errors;
            node.Scope = tour.scope;

            node.FeatureNodes.ForEach(feature => feature.Accept(tour));
        }

        public void Visit(AttributeNode node)
        {
            if (!scope.IsDefinedType(node.Formal.Type.Text, out TypeInfo type))
                errors.Add(SemanticError.NotDeclaredType(node.Formal.Type));

            if (scope.IsDefined(node.Formal.Id.Text, out TypeInfo t))
                errors.Add(SemanticError.RepeatedVariable(node.Formal.Id));

            scope.Define(node.Formal.Id.Text, type);
        }

        public void Visit(MethodNode node)
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
        public void Visit(AssignmentNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ArithmeticOperation node)
        {
            throw new NotImplementedException();
        }

        public void Visit(SequenceNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoolNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(CaseNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchExplicitNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchImplicitNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(EqualNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(ComparisonOperation node)
        {
            throw new NotImplementedException();
        }

        public void Visit(FormalNode formalNode)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IsVoidNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(LetNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NegNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NewNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(NotNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(StringNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdentifierNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(WhileNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(VoidNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(SelfNode node)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

}
