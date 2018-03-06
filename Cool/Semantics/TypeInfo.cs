using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    /*class TypeInfo : ItemInfo
    {
        public TypeInfo(string name) : base(name, Types.Void) { }

        static public bool operator ==(TypeInfo a, TypeInfo b)
        {
            return a.Equals(b) ||
                (!a.Equals(Types.Void) && !b.Equals(Types.Void) &&
                !a.Equals(Types.Int) && !b.Equals(Types.Int) &&
                (a.Equals(Types.Nil) || b.Equals(Types.Nil)));
        }

        static public bool operator !=(TypeInfo a, TypeInfo b)
        {
            return !(a == b);
        }

        public override string ToString() => Name;
    }*/
}
