using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class LabelLine : CodeLine
    {
        public string Label { get; }
        public string Head { get; }
        public string Tag { get; }
        public LabelLine(string head, string tag)
        {
            Head = head;
            Tag = tag;
            Label = Head + "." + Tag;
        }
    }
}
