using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class ArithmeticLine : CodeLine
    {
        public int AssignVariable { get; }
        public int LeftOperandVariable { get; }
        public int RightOperandVariable { get; }
        public string Operand { get; }

        public ArithmeticLine(int assign_variable, int left_operand, int right_operand, string operand)
        {
            AssignVariable = assign_variable;
            LeftOperandVariable = left_operand;
            RightOperandVariable = right_operand;
            Operand = operand;
        }

        public override string ToString()
        {
            return $"t{AssignVariable} = t{LeftOperandVariable} {Operand} t{RightOperandVariable}";
        }
    }
}
