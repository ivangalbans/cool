using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
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
    }
}
