using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    class CallLabelLine : CodeLine
    {
        LabelLine Method { get; }
        public CallLabelLine(LabelLine method)
        {
            Method = method;
        }

        public override string ToString()
        {
            return $"Call {Method.Label};";
        }
    }

    class CallAddressLine : CodeLine
    {
        int Address { get; }
        public CallAddressLine(int address)
        {
            Address = address;
        }

        public override string ToString()
        {
            return $"Call t{Address};";
        }
    }
}
