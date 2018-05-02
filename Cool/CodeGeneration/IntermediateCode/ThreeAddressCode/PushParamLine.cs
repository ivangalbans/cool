using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class PushParamLine : CodeLine
    {
        int VariableCounter;
        public PushParamLine(int variable_counter)
        {
            VariableCounter = variable_counter;
        }

        public override string ToString()
        {
            return "PushParam t" + VariableCounter + ";";
        }
    }
}
