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
        public TypeInfo Parent { get; set; } = NULL;
        public ClassNode ClassReference { get; set; }


        /// <summary>
        /// Check if a type inherit of other type in the hierarchy of the program.
        /// </summary>
        /// <param name="other">Represent the second type to check</param>
        /// <returns>True if the first type inherit of the second</returns>
        public virtual bool Inherit(TypeInfo other)
        {
            if (Name == other.Name) return true;
            return Parent.Inherit(other);
        }

        public static bool operator <=(TypeInfo a, TypeInfo b)
        {
            return a.Inherit(b);
        }

        public static bool operator >=(TypeInfo a, TypeInfo b)
        {
            return b.Inherit(a);
        }

        public static bool operator ==(TypeInfo a, TypeInfo b)
        {
            return a.Name == b.Name;
        }

        public static bool operator !=(TypeInfo a, TypeInfo b)
        {
            return !(a == b);
        }

        #region NULL
        private static NullTypeInfo nullTypeInfo = new NullTypeInfo();

        public static NullTypeInfo NULL => nullTypeInfo;

        internal class NullTypeInfo : TypeInfo
        {
            public override bool Inherit(TypeInfo other)
            {
                return false;
            }
        }
        #endregion

    }
}
