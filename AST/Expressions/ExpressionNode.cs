using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST.Scope;
using AST.Visitor;

namespace AST.Nodes.Abstract
{
    public abstract class ExpressionNode : ASTNode
    {
        public int Line { get; set; }

        public int Column { get; set; }

    }
}
