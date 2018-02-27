using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST.Nodes;
using AST.Nodes.Abstract;
using AST.Scope;
using AST.Visitor;

namespace Semantic
{
    public class VisitorAST1 : IVisitor, ISemanticChecker
    {
        public ProgramNode SemanticCheck(ProgramNode programNode)
        {
            throw new NotImplementedException();
        }

        public void Visit(ObjectNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(BlockNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(ClassNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(ProgramNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(ComparisonOperation node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(ArithmeticOperation node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(EqualNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(IntNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(StringNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(VoidNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(BoolNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(IdentifierNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchExplicitNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(DispatchImplicitNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(NegNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(NotNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(IsVoidNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(CaseNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(IfNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(WhileNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(LetNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(NewNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(AttributeNode node, IScope scope)
        {
            throw new NotImplementedException();
        }

        public void Visit(MethodNode node, IScope scope)
        {
            throw new NotImplementedException();
        }
    }
}
