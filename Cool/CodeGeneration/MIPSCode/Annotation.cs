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
        public Dictionary<string, int> StringsCounter;
        public Dictionary<string, string> Inherit;

        int current_line;
        string current_function;
        int string_counter;

        public Annotation(List<CodeLine> lines)
        {
            FunctionVarsSize = new Dictionary<string, int>();
            FunctionLimits = new Dictionary<string, (int, int)>();
            FunctionParamsCount = new Dictionary<string, int>();
            StringsCounter = new Dictionary<string, int>();
            Inherit = new Dictionary<string, string>();
            string_counter = 0;

            for (current_line = 0; current_line < lines.Count; ++current_line)
            {
                lines[current_line].Accept(this);
            }
        }

        public void Visit(LabelLine line)
        {
            if (line.Head[0] != '_')
            {
                current_function = line.Label;
                FunctionVarsSize[current_function] = 0;
                FunctionLimits[current_function] = (current_line, -1);
                FunctionParamsCount[current_function] = 0;
            }
        }

        public void Visit(AllocateLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.Variable + 1);
        }

        public void Visit(AssignmentVariableToVariableLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.Left + 1);
        }

        public void Visit(AssignmentMemoryToVariableLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.Left + 1);
        }

        public void Visit(AssignmentConstantToVariableLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.Left + 1);
        }

        public void Visit(AssignmentStringToVariableLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.Left + 1);
            if (!StringsCounter.ContainsKey(line.Right))
            {
                StringsCounter[line.Right] = string_counter++;
            }

        }

        public void Visit(AssignmentLabelToVariableLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.Left + 1);
        }

        public void Visit(BinaryOperationLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.AssignVariable + 1);
        }

        public void Visit(UnaryOperationLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.AssignVariable + 1);
        }

        public void Visit(ParamLine line)
        {
            FunctionVarsSize[current_function] = Math.Max(FunctionVarsSize[current_function], line.VariableCounter + 1);
            ++FunctionParamsCount[current_function];
        }

        public void Visit(ReturnLine line)
        {
            FunctionLimits[current_function] = (FunctionLimits[current_function].Item1, current_line);
        }

        public void Visit(AssignmentStringToMemoryLine line)
        {
            if (!StringsCounter.ContainsKey(line.Right))
            {
                StringsCounter[line.Right] = string_counter++;
            }
        }

        public void Visit(InheritLine line)
        {
            Inherit[line.Child] = line.Parent;

            if (!StringsCounter.ContainsKey(line.Child))
                StringsCounter[line.Child] = string_counter++;
            if (!StringsCounter.ContainsKey(line.Parent))
                StringsCounter[line.Parent] = string_counter++;
        }

        public void Visit(AssignmentVariableToMemoryLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(AssignmentConstantToMemoryLine line)
        {
            return;
            throw new NotImplementedException();
        }

        
        public void Visit(AssignmentLabelToMemoryLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(CallLabelLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(CallAddressLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(CommentLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(GotoJumpLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(ConditionalJumpLine line)
        {
            return;
            throw new NotImplementedException();
        }


        public void Visit(AssignmentNullToVariableLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(PopParamLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(PushParamLine line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(AssignmentInheritToVariable line)
        {
            return;
            throw new NotImplementedException();
        }

        public void Visit(SpecialObjectReturn line)
        {
            throw new NotImplementedException();
        }

        public void Visit(ReturnTypeLine line)
        {
            throw new NotImplementedException();
        }
    }
}
