using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class PopLine : CodeLine
    {
        int Times;
        public PopLine(int times)
        {
            Times = times;
        }
    }
}
