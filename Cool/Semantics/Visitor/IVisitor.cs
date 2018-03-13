using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cool.AST;

namespace Cool.Semantics
{
    interface IVisitor<TNode> where TNode : ASTNode
    {
        void Visit(TNode node, IScope scope, List<SemanticError> errors);
    }
}
