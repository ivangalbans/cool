using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    class FunctionInfo : ItemInfo
    {
        public bool IsStdl { get; }
        public TypeInfo[] Parameters { get; }
        
        /// <summary>
        /// Store the names of the visible foreign variables to the function
        /// </summary>
        public List<string> ForeignVariables { get; } = new List<string>();

        public FunctionInfo(string name, bool inStdl, TypeInfo returnType, params TypeInfo[] parameters) : base(name, returnType)
        {
            IsStdl = inStdl;
            Parameters = parameters;
        }
        
    }
}
