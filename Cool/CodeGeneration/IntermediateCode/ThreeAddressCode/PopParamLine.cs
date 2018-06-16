using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.MIPSCode;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class PopParamLine : CodeLine
    {
        int Times;
        public PopParamLine(int times)
        {
            Times = times;
        }

        public override void Accept(ICodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"PopParam {Times};" ;
        }
    }
}
