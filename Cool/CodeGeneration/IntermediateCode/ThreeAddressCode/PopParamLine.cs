using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class PopParamLine : CodeLine
    {
        int Times;
        public PopParamLine(int times)
        {
            Times = 4*times;
        }

        public override string ToString()
        {
            return $"PopParam {Times};" ;
        }
    }
}
