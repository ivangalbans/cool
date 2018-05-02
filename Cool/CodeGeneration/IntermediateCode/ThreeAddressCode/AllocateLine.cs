using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    class AllocateLine : CodeLine
    {
        int Variable { get; }
        int Size { get; }

        public AllocateLine(int variable, int size)
        {
            Variable = variable;
            Size = size;
        }

        public override string ToString()
        {
            return $"t{Variable} = Alloc {Size};";
        }
    }
}
