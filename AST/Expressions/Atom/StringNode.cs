using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class StringNode : AtomNode
    {
        public string Value { get; set; }

        public StringNode(Token intToken) : base(intToken)
        {
            Value = intToken.Text;
        }
    }
}
