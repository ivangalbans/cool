using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.AST
{

    class NullType : TypeNode
    {
        public NullType(int line = 0, int column = 0, string name = "Null-Type-Object") : base(line, column, name) { }
    }
}
