using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.MIPSCode;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class AllocateLine : CodeLine
    {
        int Variable { get; }
        int Size { get; }

        public AllocateLine(int variable, int size)
        {
            Variable = variable;
            Size = size;
        }

        public override void Accept(ICodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"t{Variable} = Alloc {Size};";
        }
    }
}
