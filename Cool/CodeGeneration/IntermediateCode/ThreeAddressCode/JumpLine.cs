using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class GotoJumpLine : CodeLine
    {
        LabelLine Label;

        public GotoJumpLine(LabelLine label)
        {
            Label = label;
        }

        public override string ToString()
        {
            return $"Goto {Label.Label}";
        }
    }

    public class ConditionalJumpLine : CodeLine
    {
        LabelLine Label;
        int ConditionalVar;
        public ConditionalJumpLine(int conditional_var, LabelLine label)
        {
            Label = label;
            ConditionalVar = conditional_var;
        }
        public override string ToString()
        {
            return $"IfZ t{ConditionalVar} Goto {Label.Label}";
        }
    }
}
