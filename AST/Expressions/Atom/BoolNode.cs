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
    public class BoolNode : AtomNode
    {
        public bool Value { get; set; }

        public BoolNode(Token boolToken) : base(boolToken)
        {
            Value = boolToken.Text.ToLower() == "true";
        }

    }
}
