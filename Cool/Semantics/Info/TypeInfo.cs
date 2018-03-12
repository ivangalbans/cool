using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    class TypeInfo
    {
        public string Name { get; set; }
        public TypeInfo Parent { get; set; }
        public ClassNode ClassReference { get; set; }


        /// <summary>
        /// Check if a type inherit of other type in the hierarchy of the program.
        /// </summary>
        /// <param name="other">Represent the second type to check</param>
        /// <returns>True if the first type inherit of the second</returns>
        public bool Inherit(TypeInfo other)
        {
            if (Name == other.Name) return true;
            return Parent is null ? false : Parent.Inherit(other);
        }

        public static bool operator <=(TypeInfo a, TypeInfo b)
        {
            return a is null || b is null ? false : a.Inherit(b);
        }

        public static bool operator >=(TypeInfo a, TypeInfo b)
        {
            return a is null || b is null ? false : a.Inherit(b);
        }

    }
}
