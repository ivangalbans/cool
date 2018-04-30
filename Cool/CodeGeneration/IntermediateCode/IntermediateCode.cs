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

            DefineVirtualTable("Object", new List<string>() {"abort", "type_name", "copy"});
            DefineAttributeTable("Object", new List<string>());

            DefineVirtualTable("IO", new List<string>() { "out_string", "out_int", "in_string", "in_int" });
            DefineAttributeTable("IO", new List<string>());

            DefineVirtualTable("String", new List<string>() { "length", "concat", "substr" });
            DefineAttributeTable("String", new List<string>());

        }

        public void DefineVirtualTable(string cclass, List<string> methods)
        {
            List<LabelLine> table = new List<LabelLine>();
            if (cclass == "Object")
            {
                methods.ForEach(m => table.Add(new LabelLine("Object", m)));
            }
            else
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                VTables[parent].ForEach(m => table.Add(m));

                foreach (var m in methods)
                {
                    int i = VTables[parent].FindIndex((x) => x.Tag == m);
                    if (i != -1)
                        table[i] = new LabelLine(cclass, m);
                    else
                        table.Add(new LabelLine(cclass, m));
                }
            }
            
            VTables[cclass] = table;
        }
        public int GetVirtualPosition(string cclass, string method)
        {
            return VTables[cclass].FindIndex((x) => x.Tag == method);
        }

        public List<LabelLine> GetVirtualTable(string cclass)
        {
            return VTables[cclass];
        }

        public void DefineAttributeTable(string cclass, List<string> attrs)
        {
            List<string> table = new List<string>();
            if (cclass != "Object")
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                ATables[parent].ForEach(m => table.Add(m));
                attrs.ForEach(m => table.Add(m));
            }

            ATables[cclass] = table;
        }

        public int GetAttributePosition(string cclass, string attr)
        {
            return ATables[cclass].FindIndex((x) => x == attr);
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
            throw new NotImplementedException();
        }
        
    }
}
