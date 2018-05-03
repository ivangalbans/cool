using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class VTableLine : CodeLine
    {
        List<LabelLine> Methods { get; }
        string cclass { get; }

        public VTableLine(string cclass, List<LabelLine> methods)
        {
            Methods = methods;
            this.cclass = cclass;
        }

        public override string ToString()
        {
            string repr = $"VTable {cclass}: ";
            foreach(var m in Methods)
            {
                repr += m.Label + ", ";
            }
            repr += ";\n";
            return repr;
        }

    }
}
