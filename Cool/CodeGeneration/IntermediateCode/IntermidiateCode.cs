using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.IntermediateCode
{
    class IntermidiateCode : IIntermediateCode
    {
        List<TypeLine> Type;
        List<DataLine> Data;
        List<CodeLine> Code;

        public void AddCodeLine(CodeLine line)
        {
            throw new NotImplementedException();
        }

        public void DefineAttributeTable(string cclass, List<string> attrs)
        {
            throw new NotImplementedException();
        }

        public void DefineStringData(string label, string texto)
        {
            throw new NotImplementedException();
        }

        public void DefineVirtualTable(string cclass, List<LabelLine> methods)
        {
            throw new NotImplementedException();
        }

        public int GetAttributePosition(string cclass, string attr)
        {
            throw new NotImplementedException();
        }

        public List<LabelLine> GetAttributeTable(string cclass)
        {
            throw new NotImplementedException();
        }

        public void GetString(string label)
        {
            throw new NotImplementedException();
        }

        public int GetVirtualPosition(string cclass, LabelLine method)
        {
            throw new NotImplementedException();
        }

        public List<LabelLine> GetVirtualTable(string cclass)
        {
            throw new NotImplementedException();
        }
    }
}
