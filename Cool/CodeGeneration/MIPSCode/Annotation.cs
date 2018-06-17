using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.MIPSCode
{
    public class Annotation : ICodeVisitor
    {
        public Dictionary<string, int> FunctionVarsSize;
        public Dictionary<string, (int, int)> FunctionLimits;
        public Dictionary<string, int> FunctionParamsCount;

        int current_line;
        string current_funtion;

        public Annotation(List<CodeLine> lines)
        {
            FunctionVarsSize = new Dictionary<string, int>();
            FunctionLimits = new Dictionary<string, (int, int)>();
            FunctionParamsCount = new Dictionary<string, int>();

            for (current_line = 0; current_line < lines.Count; ++current_line)
            {
                lines[current_line].Accept(this);
            }
        }

        public void Visit(LabelLine line)
        {
            if (!char.IsNumber(line.Tag[0]))
            {
                current_funtion = line.Label;
                FunctionVarsSize[current_funtion] = 0;
                FunctionLimits[current_funtion] = (current_line, -1);
                FunctionParamsCount[current_funtion] = 0;
            }
        }

        public void Visit(AllocateLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.Variable);
        }

        public void Visit(AssignmentVariableToVariableLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.Left);
        }

        public void Visit(AssignmentMemoryToVariableLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.Left);
        }

        public void Visit(AssignmentConstantToVariableLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.Left);
        }

        public void Visit(AssignmentStringToVariableLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.Left);
        }

        public void Visit(AssignmentLabelToVariableLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.Left);
        }

        public void Visit(BinaryOperationLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.AssignVariable);
        }

        public void Visit(UnaryOperationLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.AssignVariable);
        }

        public void Visit(ParamLine line)
        {
            FunctionVarsSize[current_funtion] = Math.Max(FunctionVarsSize[current_funtion], line.VariableCounter);
            ++FunctionParamsCount[current_funtion];
        }

        public void Visit(ReturnLine line)
        {
            FunctionLimits[current_funtion] = (FunctionLimits[current_funtion].Item1, current_line);
        }

        public void Visit(AssignmentVariableToMemoryLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentConstantToMemoryLine line)
        {
            throw new NotImplementedException();
        }

        public void Visit(AssignmentStringToMemoryLine line)
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


        public void Visit(NullLine line)
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

    }
}
