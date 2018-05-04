using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    class HeadLine : CodeLine
    {
        string Name { get; }
        int Size { get; }
        VTableLine VTable { get; }
        public HeadLine(string name, int size, VTableLine vtable)
        {
            Name = name;
            Size = size;
            VTable = vtable;
        }

        public override string ToString()
        {
            string repr = $"Head: {Name}, {Size}, VTable {Name};\n";

            return repr;
        }
    }
}
