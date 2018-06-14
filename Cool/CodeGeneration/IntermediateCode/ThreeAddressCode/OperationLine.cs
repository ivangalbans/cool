using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class BinaryOperationLine : CodeLine
    {
        public int AssignVariable { get; }
        public int LeftOperandVariable { get; }
        public int RightOperandVariable { get; }
        public string Symbol { get; }

        public BinaryOperationLine(int assign_variable, int left_operand, int right_operand, string symbol)
        {
            AssignVariable = assign_variable;
            LeftOperandVariable = left_operand;
            RightOperandVariable = right_operand;
            Symbol = symbol;
        }

        public override string ToString()
        {
            return $"t{AssignVariable} = t{LeftOperandVariable} {Symbol} t{RightOperandVariable}";
        }
    }

    public class UnaryOperationLine : CodeLine
    {
        public int AssignVariable { get; }
        public int OperandVariable { get; }
        public string Symbol { get; }

        public UnaryOperationLine(int assign_variable, int operand, string symbol)
        {
            AssignVariable = assign_variable;
            OperandVariable = operand;
            Symbol = symbol;
        }

        public override string ToString()
        {
            return $"t{AssignVariable} = {Symbol} t{OperandVariable}";
        }
    }


}
