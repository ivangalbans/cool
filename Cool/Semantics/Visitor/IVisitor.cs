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
        void Visit(ArithmeticOperation  node);
        void Visit(AssignmentNode       node);
        void Visit(AttributeNode        node);
        void Visit(BoolNode             node);
        void Visit(CaseNode             node);
        void Visit(ClassNode            node);
        void Visit(ComparisonOperation  node);
        void Visit(DispatchExplicitNode node);
        void Visit(DispatchImplicitNode node);
        void Visit(EqualNode            node);
        void Visit(FormalNode           node);
        void Visit(IdentifierNode       node);
        void Visit(IfNode               node);
        void Visit(IntNode              node);
        void Visit(IsVoidNode           node);
        void Visit(LetNode              node);
        void Visit(MethodNode           node);
        void Visit(NegNode              node);
        void Visit(NewNode              node);
        void Visit(NotNode              node);
        void Visit(ProgramNode          node);
        void Visit(SelfNode             node);
        void Visit(SequenceNode         node);
        void Visit(StringNode           node);
        void Visit(VoidNode             node);
        void Visit(WhileNode            node);
    }
}
