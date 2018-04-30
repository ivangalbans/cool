using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    
    class StringDataLine : DataLine
    {
        string Text { get; }
        string Label { get; }

        public StringDataLine(string label, string text)
        {
            Text = text;
            Label = label;
        }
    }
}
