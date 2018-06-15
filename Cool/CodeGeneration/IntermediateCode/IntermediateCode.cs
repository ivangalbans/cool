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
            VTables = new Dictionary<string, List<(string, string)>>();
            //VTables = new Dictionary<string, List<LabelLine>>();
            //ATables = new Dictionary<string, List<string>>();
            Code = new List<CodeLine>();
            //Strings = new List<(String,String)>();

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
            VTables[cclass] = new List<(string, string)>();
            //VTables[cclass] = new List<LabelLine>();
            //ATables[cclass] = new List<string>();
        }


        public void DefineClass(string cclass)
        {
            VTables[cclass] = new List<(string, string)>();
            
            if (cclass != "Object")
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                VTables[parent].ForEach(m => VTables[cclass].Add(m));
            }
        }

        public void DefineMethod(string cclass, string method)
        {
            string label = cclass + "." + method;
            if (cclass != "Object")
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                int i = VTables[parent].FindIndex((x) => x.Item2 == method);
                //keep with the same parent address for that method (use in override)
                if (i != -1)
                {
                    VTables[cclass][i] = (cclass, method);
                    return;
                }
            }

            VTables[cclass].Add((cclass, method));
        }

        public int GetOffset(string cclass, string item)
        {
            return VTables[cclass].FindIndex((x) => x.Item2 == item);
        }

        public (string, string) GetDefinition(string cclass, string item)
        {
            return VTables[cclass].Find((x) => x.Item2 == item);
        }

        public void DefineAttribute(string cclass, string attr)
        {
            if (cclass != "Object")
            {
                string parent = Scope.GetType(cclass).Parent.Text;
                int i = VTables[parent].FindIndex((x) => x.Item2 == attr);
                //keep with the same parent address
                if (i != -1)
                    return;
            }

            VTables[cclass].Add((cclass, attr));
        }
        
        public int GetSizeClass(string cclass)
        {
            return (VTables[cclass].Count + 3);
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
