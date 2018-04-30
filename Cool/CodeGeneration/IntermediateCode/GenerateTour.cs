using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.AST;
using Cool.Semantics;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.IntermediateCode
{
    class GenerateTour : IVisitor
    {
        IIntermediateCode IntermediateCode;
        IScope Scope;

        public IIntermediateCode GetIntermediateCode(ProgramNode node, IScope scope)
        {
            Scope = scope;
            node.Accept(this);
            IntermediateCode = new IntermediateCode(scope);
            return IntermediateCode;
        }

        public void Visit(ProgramNode node)
        {
            List<ClassNode> sorted = new List<ClassNode>();
            sorted.AddRange(node.Classes);
            sorted.Sort((x,y) => (Scope.GetType(x.TypeClass.Text) <= Scope.GetType(y.TypeClass.Text) ? 1 : -1));
            
            foreach (var c in sorted)
            {
                Console.WriteLine(c.TypeClass + ":" + c.TypeInherit);
                c.Accept(this);
            }

            Console.WriteLine(Scope.GetType("Main"));

            List<int> a = new List<int>() { 1, 4, 6 };
            Console.WriteLine(a.FindIndex((x) => x == 6));

            //throw new NotImplementedException();
        }

        public void Visit(ClassNode node)
        {
            //throw new NotImplementedException();
        }

        public void Visit(ArithmeticOperation node)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(AttributeNode node)
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
        

        public void Visit(ComparisonOperation node)
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

        public void Visit(FormalNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdentifierNode node)
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

        public void Visit(MethodNode node)
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

        public void Visit(SelfNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(SequenceNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(StringNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(VoidNode node)
        {
            throw new NotImplementedException();
        }

        public void Visit(WhileNode node)
        {
            throw new NotImplementedException();
        }
    }
}
