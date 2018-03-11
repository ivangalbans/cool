using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    class Scope
    {
        /// <summary>
        /// Information relative to variables and functions
        /// </summary>
        Dictionary<string, ItemInfo> _symbols;

        /// <summary>
        /// Information relative to types
        /// </summary>
        Dictionary<string, TypeInfo> _types;

        public Scope()
        {
            _symbols = new Dictionary<string, ItemInfo>();
            _types = new Dictionary<string, TypeInfo>();
        }

    }
}
