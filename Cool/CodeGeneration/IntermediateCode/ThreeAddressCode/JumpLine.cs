using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.MIPSCode;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class GotoJumpLine : CodeLine
    {
        public LabelLine Label;

        public GotoJumpLine(LabelLine label)
        {
            Label = label;
        }

        public override void Accept(ICodeVisitor visitor)
        {
            visitor.Visit(this);
        }


        public override string ToString()
        {
            return $"Goto {Label.Label}";
        }
    }

    public class ConditionalJumpLine : CodeLine
    {
        public LabelLine Label;
        public int ConditionalVar;
        public ConditionalJumpLine(int conditional_var, LabelLine label)
        {
            Label = label;
            ConditionalVar = conditional_var;
        }

        public override void Accept(ICodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"IfZ t{ConditionalVar} Goto {Label.Label}";
        }
    }
}
