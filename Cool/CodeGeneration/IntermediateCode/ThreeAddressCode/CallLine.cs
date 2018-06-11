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
        int Result { get; }
        public CallLabelLine(LabelLine method, int result_variable = -1)
        {
            Method = method;
            Result = result_variable;
        }

        public override string ToString()
        {
            if(Result == -1)
                return $"Call {Method.Label};";
            else
                return $"t{Result} = Call {Method.Label};";
        }
    }

    class CallAddressLine : CodeLine
    {
        int Address { get; }
        int Result { get; }
        public CallAddressLine(int address, int result_variable = -1)
        {
            Address = address;
            Result = result_variable;
        }

        public override string ToString()
        {
            if(Result == -1)
                return $"Call t{Address};";
            else
                return $"t{Result} = Call t{Address};";
        }
    }
}
