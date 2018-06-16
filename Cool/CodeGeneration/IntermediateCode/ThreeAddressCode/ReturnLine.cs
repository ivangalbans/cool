using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.MIPSCode;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class ReturnLine : CodeLine
    {
        int VariableCounter { get; }

        public ReturnLine(int variable_counter)
        {
            VariableCounter = variable_counter;
        }

        public override void Accept(ICodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "return " + (VariableCounter == -1 ? "" : "t" + VariableCounter.ToString()) + ";\n";
        }
    }
}
