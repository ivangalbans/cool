using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.AST
{
    class NullId : IdNode
    {
        public NullId(int line = 0, int column = 0, string name = "null-object") : base(line, column, name) { }
    }
}
