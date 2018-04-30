using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class PushLine : CodeLine
    {
        int VariableCounter;
        public PushLine(int variable_counter)
        {
            VariableCounter = variable_counter;
        }

        public override string ToString()
        {
            return "Push v" + VariableCounter + ";";
        }
    }
}
