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

        Dictionary<string, List<LabelLine>> Constructors;

        public IntermediateCode(IScope scope)
        {
            Scope = scope;
            VTables = new Dictionary<string, List<LabelLine>>();
            ATables = new Dictionary<string, List<string>>();
            Constructors = new Dictionary<string, List<LabelLine>>();
            Code = new List<CodeLine>();
            Strings = new List<StringDataLine>();

            DefineMethod("Object", "abort");
            DefineMethod("Object", "type_name");
            DefineMethod("Object", "copy");
            DefineMethod("IO", "out_string");
            DefineMethod("IO", "out_int");
            DefineMethod("IO", "in_string");
            DefineMethod("IO", "in_int");
            DefineMethod("String", "length");
            DefineMethod("String", "concat");
            DefineMethod("String", "substr");

        }

        void Init(string cclass)
        {
            VTables[cclass] = new List<LabelLine>();
            ATables[cclass] = new List<string>();
            Constructors[cclass] = new List<LabelLine>();
        }

        public void DefineMethod(string cclass, string method)
        {
            if(!VTables.ContainsKey(cclass))
            {
                Init(cclass);
                if (cclass != "Object")
                {
                    string parent = Scope.GetType(cclass).Parent.Text;
                    VTables[parent].ForEach(m => VTables[cclass].Add(m));
                }
            }

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

        public List<LabelLine> GetVirtualTable(string cclass)
        {
            return VTables[cclass];
        }

        public void DefineAttribute(string cclass, string attr)
        {
            if (!ATables.ContainsKey(cclass))
            {
                Init(cclass);
                if (cclass != "Object")
                {
                    string parent = Scope.GetType(cclass).Parent.Text;
                    ATables[parent].ForEach(m => ATables[cclass].Add(m));
                }
            }

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
            if (index != -1) return 4 * (index + 2);
            else return -1;
        }

        public List<string> GetAttributeTable(string cclass)
        {
            return ATables[cclass];
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
            return Code;
            throw new NotImplementedException();
        }

        public LabelLine AddConstructorCallAttribute(string cclass, string attr)
        {
            LabelLine label = new LabelLine(cclass + ".constructor", "set_" + attr);
            Constructors[cclass].Add(label);
            return label;
        }
    }
}
