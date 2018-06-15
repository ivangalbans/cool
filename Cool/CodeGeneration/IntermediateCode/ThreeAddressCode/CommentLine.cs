using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.CodeGeneration.IntermediateCode.ThreeAddressCode
{
    class CommentLine : CodeLine
    {
        string Comment { get; }
        public CommentLine(string comment)
        {
            Comment = comment;
        }

        public override string ToString()
        {
            return "// " + Comment;
        }
    }
}
