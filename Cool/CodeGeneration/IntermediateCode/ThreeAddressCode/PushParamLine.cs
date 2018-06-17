using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.MIPSCode;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class PushParamLine : CodeLine
    {
        public int Variable;
        public PushParamLine(int variable)
        {
            Variable = variable;
        }

        public override void Accept(ICodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "PushParam t" + Variable + ";";
        }
    }
}
