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

            foreach (var str in annotation.StringsCounter)
            {
                if (str.Key[0] == '\"')
                    Data.Add($"str{str.Value}: .asciiz \"{str.Key.Substring(1, str.Key.Length - 2)}\"");
                else
                    Data.Add($"str{str.Value}: .asciiz \"{str.Key}\"");
            }

            for (int i = 0; i < lines.Count; ++i)
            {
                lines[i].Accept(this);
            }

            string gen = "";

            //System.IO.File.ReadAllText("Code.txt");

            gen += ".data\n";
            gen += "buffer: .space 65536\n";
            foreach (string s in Data)
                gen += s + "\n";

            gen += "\n.globl main\n";
            gen += ".text\n";
            gen += "\nmain:\n";

            foreach (string s in Code)
                gen += s + "\n";



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
            Code.Add($"sw $v0, {4*line.Variable}($sp)");
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
            Code.Add($"sw $a1, {line.Left * 4}($sp)");
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
            Code.Add($"addi $sp, $sp, {-(size + 1)*4}");
            Code.Add($"jal {line.Method.Label}");
            Code.Add($"addi $sp, $sp, {(size + 1) * 4}");
            Code.Add($"lw $ra, {-size*4}($sp)");
            if(line.Result != -1)
                Code.Add($"sw $v0, {-line.Result*4}($sp)");
        }

        public void Visit(CallAddressLine line)
        {
            Code.Add($"sw $ra, {-size * 4}($sp)");
            Code.Add($"addi $sp, $sp, {-(size + 1) * 4}");
            Code.Add($"lw $a0, {-line.Address * 4}($sp)");
            Code.Add($"jalr $ra, $a0");
            Code.Add($"addi $sp, $sp, {(size + 1) * 4}");
            Code.Add($"lw $ra, {-size * 4}($sp)");
            if (line.Result != -1)
                Code.Add($"sw $v0, {-line.Result * 4}($sp)");
        }
        

        public void Visit(BinaryOperationLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(UnaryOperationLine line)
        {
            throw new NotImplementedException();
        }


    }
}
