using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grammars;

namespace AST.Nodes.Abstract
{
    public abstract class AtomNode : ExpressionNode
    {
        public AtomNode(Token atomToken)
        {
            Line = atomToken.Line;
            Column = atomToken.Column;
        }
    }
}
