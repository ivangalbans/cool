using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.MIPSCode;


namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public abstract class CodeLine
    {
        public abstract void Accept(ICodeVisitor visitor);
    }

}
