using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AST.Nodes.Abstract;
using Grammars;

namespace AST.Nodes
{
    public class VoidNode : AtomNode
    {
        public VoidNode(Token voidToken) : base(voidToken) { }
    }
}
