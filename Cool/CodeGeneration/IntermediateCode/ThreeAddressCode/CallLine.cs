using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    class CallLine : CodeLine
    {
        LabelLine Method { get; }
        public CallLine(LabelLine method)
        {
            Method = method;
        }

        public override string ToString()
        {
            return $"Call {Method.Label};";
        }
    }
}
