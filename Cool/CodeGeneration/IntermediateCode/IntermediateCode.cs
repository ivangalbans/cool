using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.CodeGeneration.IntermediateCode.ThreeAddressCode;
using Cool.Semantics;

namespace Cool.CodeGeneration.IntermediateCode
{
    public class IntermediateCode : IIntermediateCode
    {
        IScope Scope;
        //List<TypeLine> Type;
        List<StringDataLine> Strings;
        List<CodeLine> Code;

        Dictionary<string, List<LabelLine>> VTables;
        Dictionary<string, List<string>> ATables;
        

        public IntermediateCode(IScope scope)
        {
            Scope = scope;
            VTables = new Dictionary<string, List<LabelLine>>();
            ATables = new Dictionary<string, List<string>>();
            Code = new List<CodeLine>();
            Strings = new List<StringDataLine>();

            DefineClass("Object");
            DefineMethod("Object", "abort");
            DefineMethod("Object", "type_name");
            DefineMethod("Object", "copy");
            DefineClass("IO");
            DefineMethod("IO", "out_string");
            DefineMethod("IO", "out_int");
            DefineMethod("IO", "in_string");
            DefineMethod("IO", "in_int");
            DefineClass("String");
            DefineMethod("String", "length");
            DefineMethod("String", "concat");
            DefineMethod("String", "substr");
        }

        void Init(string cclass)
        {
            VTables[cclass] = new List<LabelLine>();
            ATables[cclass] = new List<string>();
        }

        public void DefineClass(string cclass)
        {
            VTables[cclass] = new List<LabelLine>();
            ATables[cclass] = new List<string>();
            
            if (cclass != "Object")
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                VTables[parent].ForEach(m => VTables[cclass].Add(m));
                ATables[parent].ForEach(m => ATables[cclass].Add(m));
            }
        }

        public void DefineMethod(string cclass, string method)
        {
            if (cclass != "Object")
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                int i = VTables[parent].FindIndex((x) => x.Tag == method);
                if (i != -1)
                {
                    VTables[cclass][i] = new LabelLine(cclass, method);
                    return;
                }
            }

            VTables[cclass].Add(new LabelLine(cclass, method));
        }
        public int GetMethodPosition(string cclass, string method)
        {
            return VTables[cclass].FindIndex((x) => x.Tag == method);
        }

        public LabelLine GetMethodLabel(string cclass, string method)
        {
            return VTables[cclass].Find((x) => x.Tag == method);
        }

        public VTableLine GetVirtualTable(string cclass)
        {
            return new VTableLine(cclass, VTables[cclass]);
        }
        
        public void DefineAttribute(string cclass, string attr)
        {
            if (cclass != "Object")
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                int i = ATables[parent].FindIndex((x) => x == attr);
                if (i != -1)
                {
                    return;
                }
            }

            ATables[cclass].Add(attr);
        }

        public int GetAttributePosition(string cclass, string attr)
        {
            return ATables[cclass].FindIndex((x) => x == attr);
        }

        public int GetAttributeOffset(string cclass, string attr)
        {
            int index = GetAttributePosition(cclass, attr);
            if (index != -1) return 4 * (index + 3);
            else return -1;
        }

        public List<string> GetAttributeTable(string cclass)
        {
            return ATables[cclass];
        }

        public int GetSizeClass(string cclass)
        {
            return (ATables[cclass].Count + 3) * 4;
        }

        public void DefineStringData(string label, string texto)
        {
            Strings.Add(new StringDataLine(label, texto));
        }

        public string GetString(string label)
        {
            return Strings.Find((x) => x.Label == label).Text;
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
        
    }
}
