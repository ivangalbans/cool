using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    class LabelLine : ThreeAddressCodeLine
    {
        public string Label { get; }
        public LabelLine(string label)
        {
            Label = label;
        }
    }
}
