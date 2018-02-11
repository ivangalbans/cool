using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class BoolNode : AtomNode
    {
        public bool Value { get; set; }

        public BoolNode(Token intToken) : base(intToken)
        {
            Value = intToken.Text.ToLower() == "true";
        }
    }
}
