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
    public class StringNode : AtomNode
    {
        public string Value { get; set; }

        public StringNode(Token stringToken) : base(stringToken)
        {
            Value = stringToken.Text;
        }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}
