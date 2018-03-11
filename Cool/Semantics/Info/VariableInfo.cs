using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    class VariableInfo : ItemInfo
    {
        public bool IsForeign { get; }

        public VariableInfo(string name, TypeInfo type, bool isForeign) : base(name, type)
        {
            IsForeign = isForeign;
        }
    }
}
