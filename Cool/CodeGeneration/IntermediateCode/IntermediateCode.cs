using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;
using Cool.Semantics;

namespace Cool.CodeGeneration.IntermediateCode
{
    public class IntermediateCode //: IIntermediateCode
    {
        IScope Scope;
        List<CodeLine> Code;
        Dictionary<string, List<(string, string)>> VTables;

        public IntermediateCode(IScope scope)
        {
            Scope = scope;
            Code = new List<CodeLine>();
        }
        
        
        public void AddCodeLine(CodeLine line)
        {
            Code.Add(line);
        }

        public List<CodeLine> GetCode()
        {
            List<CodeLine> code = new List<CodeLine>();

            foreach (var c in Code)
            {
                code.Add(c);
            }

            return code;
        }

        public int CountLines()
        {
            return Code.Count;
        }
        
    }
}
