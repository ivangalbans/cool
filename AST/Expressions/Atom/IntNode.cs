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
    public class IntNode : AtomNode
    {
        public int Value { get; set; }

        public IntNode(Token intToken) : base(intToken)
        {
            Value = int.Parse(intToken.Text);
        }

        public override void Accept(IVisitor visitor, IScope scope)
        {
            visitor.Visit(this, scope);
        }
    }
}
