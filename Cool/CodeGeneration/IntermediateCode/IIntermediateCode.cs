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

        string GetString(string label);

        void DefineClass(string cclass);

        void DefineMethod(string cclass, string method);

        VTableLine GetVirtualTable(string cclass);

        int GetMethodPosition(string cclass, string method);

        LabelLine GetMethodLabel(string cclass, string method);

        void DefineAttribute(string cclass, string attr);

        List<string> GetAttributeTable(string cclass);

        int GetSizeClass(string cclass);

        int GetAttributePosition(string cclass, string attr);

        int GetAttributeOffset(string cclass, string attr);

        void AddCodeLine(CodeLine line);

        List<CodeLine> GetCode();
        
    }
}
