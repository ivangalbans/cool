using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    public class Label
    {
        public string Head { get; }
        public string Tag { get; }

        public Label(string head, string tag = "")
        {
            Head = head;
            Tag = tag;
        }

        //public string Label()
        //{
        //    if (Tag != "")
        //        return Head + "." + Tag;
        //    else
        //        return Head;
        //}
    }

    public class LabelLine : CodeLine
    {
        public string Head { get; }
        public string Tag { get; }

        public string Label {
            get {
                if (Tag != "")
                    return Head + "." + Tag;
                else
                    return Head;
            }
        }

        public LabelLine(string head, string tag = "")
        {
            Head = head;
            Tag = tag;
        }

        public override string ToString()
        {
            return Label + ":";
            //return LabelLine.Label(this) + ":";
        }

        //public static string Label(LabelLine label)
        //{
        //    if (label.Tag != "")
        //        return label.Head + "." + label.Tag;
        //    else
        //        return label.Head;
        //}
    }
}
