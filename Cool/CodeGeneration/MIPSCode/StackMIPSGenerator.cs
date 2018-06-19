using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;
using System.IO;

namespace Cool.CodeGeneration.MIPSCode
{
    class StackMIPSGenerator : IMIPSCodeGenerator, ICodeVisitor
    {
        List<string> Code;
        List<string> Data;
        string current_function;
        int size;
        int param_count;
        Annotation annotation;

        public string GenerateCode(List<CodeLine> lines)
        {
            Code = new List<string>();
            Data = new List<string>();
            param_count = 0;
            annotation = new Annotation(lines);

            foreach(var x in annotation.FunctionParamsCount)
                Console.WriteLine($"{x.Key} {x.Value}");

            foreach (var str in annotation.StringsCounter)
            {
                if (str.Key.Length > 0 && str.Key[0] == '\"')
                    Data.Add($"str{str.Value}: .asciiz \"{str.Key.Substring(1, str.Key.Length - 2)}\"");
                else
                    Data.Add($"str{str.Value}: .asciiz \"{str.Key}\"");
            }

            for (int i = 0; i < lines.Count; ++i)
            {
                Code.Add($"# {lines[i]}");
                lines[i].Accept(this);
            }

            List<string> t = new List<string>();
            foreach (var s in Code)
            {
                t.Add(s);
            }

            Code = t;

            string gen = "";

            gen += ".data\n";
            gen += "buffer: .space 65536\n";
            foreach (string s in Data)
                gen += s + "\n";

            gen += "\n.globl main\n";
            gen += ".text\n";

            gen += "Object.type_name:\n";
            gen += "lw $a0, 0($sp)\n";
            gen += "lw $v0, 0($a0)\n";
            gen += "jr $ra\n";
            gen += "\n";

            gen += "Object.copy:\n";
            gen += "Object.abort:\n";
            gen += "li $v0, 10\n";
            gen += "syscall\n";
            gen += "\n";

            gen += "IO.out_string:\n";
            gen += "li $v0, 4\n";
            gen += "lw $a0, -4($sp)\n";
            gen += "syscall\n";
            gen += "jr $ra\n";
            gen += "\n";

            gen += "IO.out_int:\n";
            gen += "li $v0, 1\n";
            gen += "lw $a0, -4($sp)\n";
            gen += "syscall\n";
            gen += "jr $ra\n";
            gen += "\n";

            gen += "IO.in_string:\n";
            gen += "move $a3, $ra\n";
            gen += "la $a0, buffer\n";
            gen += "li $a1, 65536\n";
            gen += "li $v0, 8\n";
            gen += "syscall\n";
            gen += "addiu $sp, $sp, -4\n";
            gen += "sw $a0, 0($sp)\n";
            gen += "jal String.length\n";
            gen += "addiu $sp, $sp, 4\n";
            gen += "move $a2, $v0\n";
            gen += "addiu $a2, $a2, -1\n";
            gen += "move $a0, $v0\n";
            gen += "li $v0, 9\n";
            gen += "syscall\n";
            gen += "move $v1, $v0\n";
            gen += "la $a0, buffer\n";
            gen += "_io.in_string.loop:\n";
            gen += "beqz $a2, _io.in_string.end\n";
            gen += "lb $a1, 0($a0)\n";
            gen += "sb $a1, 0($v1)\n";
            gen += "addiu $a0, $a0, 1\n";
            gen += "addiu $v1, $v1, 1\n";
            gen += "addiu $a2, $a2, -1\n";
            gen += "j _io.in_string.loop\n";
            gen += "_io.in_string.end:\n";
            gen += "sb $zero, 0($v1)\n";
            gen += "move $ra, $a3\n";
            gen += "jr $ra\n";
            gen += "\n";

            gen += "IO.in_int:\n";
            gen += "li $v0, 5\n";
            gen += "syscall\n";
            gen += "jr $ra\n";
            gen += "\n";

            gen += "String.length:\n";
            gen += "lw $a0, 0($sp)\n";
            gen += "_str.length.loop:\n";
            gen += "lb $a1, 0($a0)\n";
            gen += "beqz $a1, _str.length.end\n";
            gen += "addiu $a0, $a0, 1\n";
            gen += "j _str.length.loop\n";
            gen += "_str.length.end:\n";
            gen += "lw $a1, 0($sp)\n";
            gen += "subu $v0, $a0, $a1\n";
            gen += "jr $ra\n";
            gen += "\n";

            gen += "String.concat:\n";
            gen += "move $a2, $ra\n";
            gen += "jal String.length\n";
            gen += "move $v1, $v0\n";
            gen += "addiu $sp, $sp, -4\n";
            gen += "jal String.length\n";
            gen += "addiu $sp, $sp, 4\n";
            gen += "add $v1, $v1, $v0\n";
            gen += "addi $v1, $v1, 1\n";
            gen += "li $v0, 9\n";
            gen += "move $a0, $v1\n";
            gen += "syscall\n";
            gen += "move $v1, $v0\n";
            gen += "lw $a0, 0($sp)\n";
            gen += "_str.concat.loop1:\n";
            gen += "lb $a1, 0($a0)\n";
            gen += "beqz $a1, _str.concat.end1\n";
            gen += "sb $a1, 0($v1)\n";
            gen += "addiu $a0, $a0, 1\n";
            gen += "addiu $v1, $v1, 1\n";
            gen += "j _str.concat.loop1\n";
            gen += "_str.concat.end1:\n";
            gen += "lw $a0, -4($sp)\n";
            gen += "_str.concat.loop2:\n";
            gen += "lb $a1, 0($a0)\n";
            gen += "beqz $a1, _str.concat.end2\n";
            gen += "sb $a1, 0($v1)\n";
            gen += "addiu $a0, $a0, 1\n";
            gen += "addiu $v1, $v1, 1\n";
            gen += "j _str.concat.loop2\n";
            gen += "_str.concat.end2:\n";
            gen += "sb $zero, 0($v1)\n";
            gen += "move $ra, $a2\n";
            gen += "jr $ra\n";
            gen += "\n";


            gen += "String.substr:\n";
            gen += "lw $a0, -8($sp)\n";
            gen += "addiu $a0, $a0, 1\n";
            gen += "li $v0, 9\n";
            gen += "syscall\n";
            gen += "move $v1, $v0\n";
            gen += "lw $a0, 0($sp)\n";
            gen += "lw $a1, -4($sp)\n";
            gen += "add $a0, $a0, $a1\n";
            gen += "lw $a2, -8($sp)\n";
            gen += "_str.substr.loop:\n";
            gen += "beqz $a2, _str.substr.end\n";
            gen += "lb $a1, 0($a0)\n";
            gen += "sb $a1, 0($v1)\n";
            gen += "addiu $a0, $a0, 1\n";
            gen += "addiu $v1, $v1, 1\n";
            gen += "addiu $a2, $a2, -1\n";
            gen += "j _str.substr.loop\n";
            gen += "_str.substr.end:\n";
            gen += "sb $zero, 0($v1)\n";
            gen += "jr $ra\n";
            gen += "\n";






            gen += "li $v0, 4\n";
            gen += "la $a0, str0\n";
            gen += "syscall\n";
            gen += "jr $ra\n";
            
            gen += "\nmain:\n";

            foreach (string s in Code)
                gen += s + "\n";


            gen += "li $v0, 10\n";
            gen += "syscall\n";

            return gen;
        }

        public void Visit(LabelLine line)
        {
            if (line.Head[0] != '_')
            {
                current_function = line.Label;
                size = annotation.FunctionVarsSize[current_function];
            }
            Code.Add($"\n");
            Code.Add($"{line.Label}:");
        }

        public void Visit(AllocateLine line)
        {
            Code.Add($"# Begin Allocate");
            Code.Add($"li $v0, 9");
            Code.Add($"li $a0, {4*line.Size}");
            Code.Add($"syscall");
            Code.Add($"sw $v0, {-4*line.Variable}($sp)");
            Code.Add($"# End Allocate");
        }

        public void Visit(GotoJumpLine line)
        {
            Code.Add($"j {line.Label.Label}");
        }

        public void Visit(CommentLine line)
        {
            return;
        }

        public void Visit(AssignmentVariableToMemoryLine line)
        {
            Code.Add($"lw $a0, {-line.Right*4}($sp)");
            Code.Add($"lw $a1, {-line.Left*4}($sp)");
            Code.Add($"sw $a0, {line.Offset*4}($a1)");
        }

        public void Visit(AssignmentVariableToVariableLine line)
        {
            Code.Add($"lw $a0, {-line.Right*4}($sp)");
            Code.Add($"sw $a0, {-line.Left*4}($sp)");
        }

        public void Visit(AssignmentConstantToMemoryLine line)
        {
            Code.Add($"lw $a0, {-line.Left * 4}($sp)");
            Code.Add($"li $a1, {line.Right}");
            Code.Add($"sw $a1, {-line.Offset * 4}($a0)");
        }

        public void Visit(AssignmentMemoryToVariableLine line)
        {
            Code.Add($"lw $a0, {-line.Right * 4}($sp)");
            Code.Add($"lw $a1, {line.Offset * 4}($a0)");
            Code.Add($"sw $a1, {-line.Left * 4}($sp)");
        }

        public void Visit(AssignmentConstantToVariableLine line)
        {
            Code.Add($"li $a0, {line.Right}");
            Code.Add($"sw $a0, {-line.Left * 4}($sp)");
        }

        public void Visit(AssignmentStringToVariableLine line)
        {
            Code.Add($"la $a0, str{annotation.StringsCounter[line.Right]}");
            Code.Add($"sw $a0, {-line.Left * 4}($sp)");
        }

        public void Visit(AssignmentStringToMemoryLine line)
        {
            Code.Add($"la $a0, str{annotation.StringsCounter[line.Right]}");
            Code.Add($"lw $a1, {-line.Left}($sp)");
            Code.Add($"sw $a0, {line.Offset * 4}($a1)");
        }


        public void Visit(AssignmentLabelToVariableLine line)
        {
            Code.Add($"la $a0, {line.Right.Label}");
            Code.Add($"sw $a0, {-line.Left * 4}($sp)");
        }

        public void Visit(AssignmentLabelToMemoryLine line)
        {
            Code.Add($"la $a0, {line.Right.Label}");
            Code.Add($"lw $a1, {-line.Left}($sp)");
            Code.Add($"sw $a0, {line.Offset * 4}($a1)");
        }
        public void Visit(AssignmentNullToVariableLine line)
        {
            Code.Add($"sw $zero, {-line.Variable * 4}($sp)");
        }

        public void Visit(ReturnLine line)
        {
            Code.Add($"lw $v0, {-line.Variable * 4}($sp)");
            Code.Add($"jr $ra");
        }

        public void Visit(ParamLine line)
        {
            return;
        }

        public void Visit(PopParamLine line)
        {
            param_count = 0;
        }

        public void Visit(ConditionalJumpLine line)
        {
            Code.Add($"lw $a0, {-line.ConditionalVar * 4}($sp)");
            Code.Add($"beqz $a0, {line.Label.Label}");
        }

        public void Visit(PushParamLine line)
        {
            ++param_count;
            Code.Add($"lw $a0, {-line.Variable * 4}($sp)");
            Code.Add($"sw $a0, {-(size + param_count) * 4}($sp)");
        }

        public void Visit(CallLabelLine line)
        {
            Code.Add($"sw $ra, {-size*4}($sp)");
            Code.Add($"addiu $sp, $sp, {-(size + 1)*4}");
            Code.Add($"jal {line.Method.Label}");
            //Code.Add($"nop");
            Code.Add($"addiu $sp, $sp, {(size + 1) * 4}");
            Code.Add($"lw $ra, {-size*4}($sp)");
            if(line.Result != -1)
                Code.Add($"sw $v0, {-line.Result*4}($sp)");
        }

        public void Visit(CallAddressLine line)
        {
            Code.Add($"sw $ra, {-size * 4}($sp)");
            Code.Add($"lw $a0, {-line.Address * 4}($sp)");
            Code.Add($"addiu $sp, $sp, {-(size + 1) * 4}");
            Code.Add($"jalr $ra, $a0");
            //Code.Add($"nop");
            Code.Add($"addiu $sp, $sp, {(size + 1) * 4}");
            Code.Add($"lw $ra, {-size * 4}($sp)");
            if (line.Result != -1)
                Code.Add($"sw $v0, {-line.Result * 4}($sp)");
        }
        

        public void Visit(BinaryOperationLine line)
        {
            Code.Add($"lw $a0, {-line.LeftOperandVariable * 4}($sp)");
            Code.Add($"lw $a1, {-line.RightOperandVariable * 4}($sp)");

            switch (line.Symbol)
            {
                case "+":
                    Code.Add($"add $a0, $a0, $a1");
                    break;
                case "-":
                    Code.Add($"sub $a0, $a0, $a1");
                    break;
                case "*":
                    Code.Add($"mult $a0, $a1");
                    Code.Add($"mflo $a0");
                    break;
                case "/":
                    Code.Add($"div $a0, $a1");
                    Code.Add($"mflo $a0");
                    break;
                case "<":
                    Code.Add($"sge $a0, $a0, $a1");
                    Code.Add($"li $a1, 1");
                    Code.Add($"sub $a0, $a1, $a0");
                    break;
                case "<=":
                    Code.Add($"sle $a0, $a0, $a1");
                    break;
                case "=":
                    Code.Add($"seq $a0, $a0, $a1");
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            Code.Add($"sw $a0, {-line.AssignVariable * 4}($sp)");
        }

        public void Visit(UnaryOperationLine line)
        {
            Code.Add($"lw $a0, {-line.OperandVariable * 4}($sp)");

            switch (line.Symbol)
            {
                case "not":
                    Code.Add($"li $a1, 1");
                    Code.Add($"sub $a0, $a1, $a0");
                    break;
                case "isvoid":
                    Code.Add($"seq $a0, $a0, $zero");
                    break;
                case "~":
                    Code.Add($"not $a0, $a0");
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            Code.Add($"sw $a0, {-line.AssignVariable * 4}($sp)");
        }

        public void Visit(InheritLine line)
        {

            //throw new NotImplementedException();
        }

        public void Visit(AssignmentInheritToVariable line)
        {
            //throw new NotImplementedException();
        }
    }
}
