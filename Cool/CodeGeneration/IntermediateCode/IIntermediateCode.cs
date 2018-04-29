using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;

namespace Cool.CodeGeneration.IntermediateCode
{
    interface IIntermediateCode
    {

        void DefineStringData(string label, string texto);

        void GetString(string label);

        void DefineVirtualTable(string cclass, List<LabelLine> methods);

        List<LabelLine> GetVirtualTable(string cclass);

        int GetVirtualPosition(string cclass, LabelLine method);

        void DefineAttributeTable(string cclass, List<string> attrs);

        List<LabelLine> GetAttributeTable(string cclass);

        int GetAttributePosition(string cclass, string attr);

        void AddCodeLine(CodeLine line);


    }
}
