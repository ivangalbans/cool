using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.AST
{
    class NullExpression : ExpressionNode
    {
        public NullExpression(int line = 0, int column = 0) : base(line, column) { }
    }
}
