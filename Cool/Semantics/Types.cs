using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    public static class Types
    {
        public static TypeInfo Int { get; } = new TypeInfo("int");

        public static TypeInfo String { get; } = new TypeInfo("string");

        public static TypeInfo Boolean { get; } = new TypeInfo("bool");

        public static TypeInfo Void { get; } = new TypeInfo("Void");
    }
}
