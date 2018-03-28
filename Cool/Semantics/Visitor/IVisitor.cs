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
        void Visit(ArithmeticOperation  node, IScope scope, ICollection<string> errors);
        void Visit(AssignmentNode       node, IScope scope, ICollection<string> errors);
        void Visit(AttributeNode        node, IScope scope, ICollection<string> errors);
        void Visit(BoolNode             node, IScope scope, ICollection<string> errors);
        void Visit(CaseNode             node, IScope scope, ICollection<string> errors);
        void Visit(ClassNode            node, IScope scope, ICollection<string> errors);
        void Visit(ComparisonOperation  node, IScope scope, ICollection<string> errors);
        void Visit(DispatchExplicitNode node, IScope scope, ICollection<string> errors);
        void Visit(DispatchImplicitNode node, IScope scope, ICollection<string> errors);
        void Visit(EqualNode            node, IScope scope, ICollection<string> errors);
        void Visit(FormalNode           node, IScope scope, ICollection<string> errors);
        void Visit(IdentifierNode       node, IScope scope, ICollection<string> errors);
        void Visit(IfNode               node, IScope scope, ICollection<string> errors);
        void Visit(IntNode              node, IScope scope, ICollection<string> errors);
        void Visit(IsVoidNode           node, IScope scope, ICollection<string> errors);
        void Visit(LetNode              node, IScope scope, ICollection<string> errors);
        void Visit(MethodNode           node, IScope scope, ICollection<string> errors);
        void Visit(NegNode              node, IScope scope, ICollection<string> errors);
        void Visit(NewNode              node, IScope scope, ICollection<string> errors);
        void Visit(NotNode              node, IScope scope, ICollection<string> errors);
        void Visit(ProgramNode          node, IScope scope, ICollection<string> errors);
        void Visit(SelfNode             node, IScope scope, ICollection<string> errors);
        void Visit(SequenceNode         node, IScope scope, ICollection<string> errors);
        void Visit(StringNode           node, IScope scope, ICollection<string> errors);
        void Visit(VoidNode             node, IScope scope, ICollection<string> errors);
        void Visit(WhileNode            node, IScope scope, ICollection<string> errors);
    }
}
