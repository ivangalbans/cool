using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class ReturnLine : CodeLine
    {
        int VariableCounter;

        public ReturnLine(int variable_counter)
        {
            VariableCounter = variable_counter;
        }
    }
}
