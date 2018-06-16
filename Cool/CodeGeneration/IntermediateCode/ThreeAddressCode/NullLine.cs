using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.MIPSCode;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class NullLine : CodeLine
    {
        int Variable { get; }

        public NullLine(int variable)
        {
            Variable = variable;
        }

        public override void Accept(ICodeVisitor visitor)
        {
            visitor.Visit(this);
        }


        public override string ToString()
        {
            return $"t{Variable} = NULL;";
        }
    }
}
