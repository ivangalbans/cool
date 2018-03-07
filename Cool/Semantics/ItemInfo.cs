using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    public abstract class ItemInfo
    {
        public string Name { get; }
        public TypeInfo Type { get; }
        
        protected ItemInfo(string name, TypeInfo type)
        {
            Name = name;
            Type = type;
        }
    }
}
