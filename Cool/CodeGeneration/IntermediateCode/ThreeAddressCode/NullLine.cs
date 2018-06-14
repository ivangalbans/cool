using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    class NullLine : CodeLine
    {
        int Variable { get; }

        public NullLine(int variable)
        {
            Variable = variable;
        }

        public override string ToString()
        {
            return $"t{Variable} = NULL;";
        }
    }
}
