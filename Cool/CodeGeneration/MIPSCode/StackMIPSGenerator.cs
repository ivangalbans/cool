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
        Dictionary<string, int> MaxVar;
        string current_function;

        public string GenerateCode(List<CodeLine> lines)
        {
            Code = new List<string>();
            Data = new List<string>();

            for (int i = 0; i < lines.Count; ++i)
            {

                //lines[i].Accept(this);
            }

            string gen = "";

            System.IO.File.ReadAllText("Code.txt");

            foreach (string s in Data)
                gen += s + "\n";
            
            foreach (string s in Code)
                gen += s + "\n";



            return gen;
        }

        public void Visit(AllocateLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentVariableToMemoryLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentVariableToVariableLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentConstantToMemoryLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentMemoryToVariableLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentConstantToVariableLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentStringToVariableLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentStringToMemoryLine line)
        {
            throw new NotImplementedException();
        }


        public void Visit(AssignmentLabelToVariableLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentLabelToMemoryLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(CallLabelLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(CallAddressLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(CommentLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(GotoJumpLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(ConditionalJumpLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(LabelLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(NullLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(BinaryOperationLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(UnaryOperationLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(ParamLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(PopParamLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(PushParamLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(ReturnLine line)
        {
            throw new NotImplementedException();
        }
    }
}
