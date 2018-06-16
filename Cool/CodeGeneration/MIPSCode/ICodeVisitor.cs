using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.MIPSCode
{
    public interface ICodeVisitor
    {
        void Visit(CodeLine line);
    }
}
