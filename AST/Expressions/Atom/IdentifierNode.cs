using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using AST.Scope;
using AST.Visitor;
using Grammars;

namespace AST.Nodes
{
    public class IdentifierNode : AtomNode
    {
        public string Name { get; set; }

        public IdentifierNode(Token identifierToken) : base(identifierToken)
        {
            Name = identifierToken.Text;
        }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}
