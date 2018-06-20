using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cool.Semantics;

namespace Cool.CodeGeneration.IntermediateCode
{
    public class VirtualTable
    {
        IScope Scope;
        Dictionary<string, List<(string, string)>> VTables;

        public static List<string> Object = new List<string> { "abort", "type_name", "copy" };
        public static List<string> IO = new List<string> { "out_string", "out_int", "in_string", "in_int" };
        public static List<string> String = new List<string> { "length", "concat", "substr" };

        public VirtualTable(IScope scope)
        {
            Scope = scope;
            VTables = new Dictionary<string, List<(string, string)>>();

            DefineClass("Object");
            foreach (var f in Object)
                DefineMethod("Object", f);
            
            DefineClass("IO");
            foreach (var f in IO)
                DefineMethod("IO", f);

            DefineClass("String");
            foreach (var f in String)
                DefineMethod("String", f);

            DefineClass("Int");
            DefineClass("Bool");
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
            return VTables[cclass].FindIndex((x) => x.Item2 == item) + 3;
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

    }
}
