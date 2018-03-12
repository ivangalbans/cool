using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    class Scope : IScope
    {
        /// <summary>
        /// Information relative to variables.
        /// </summary>
        public Dictionary<string, TypeInfo> Variables = new Dictionary<string, TypeInfo>();

        /// <summary>
        /// Information relative to variables.
        /// </summary>
        public Dictionary<string, (TypeInfo[], TypeInfo)> Functions = new Dictionary<string, (TypeInfo[], TypeInfo)>();

        /// <summary>
        /// Information relative to types in the current scope.
        /// </summary>
        public static Dictionary<string, TypeInfo> DeclaredTypes;

        public IScope Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TypeInfo Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        static Scope()
        {
            DeclaredTypes = new Dictionary<string, TypeInfo>();
            DeclaredTypes.Add("Object", new TypeInfo { Name = "Object", Parent = null });
            DeclaredTypes.Add("Int", new TypeInfo { Name = "Int", Parent = DeclaredTypes["Object"] });
            DeclaredTypes.Add("Bool", new TypeInfo { Name = "Bool", Parent = DeclaredTypes["Object"] });
            DeclaredTypes.Add("String", new TypeInfo { Name = "String", Parent = DeclaredTypes["Object"] });
            DeclaredTypes.Add("IO", new TypeInfo { Name = "IO", Parent = DeclaredTypes["Object"] });
        }

        public bool IsDefined(string name, out TypeInfo type)
        {
            throw new NotImplementedException();
        }

        public bool IsDefined(string name, TypeInfo[] args, out TypeInfo type)
        {
            throw new NotImplementedException();
        }

        public bool IsDefinedType(string name, out TypeInfo type)
        {
            throw new NotImplementedException();
        }

        public bool Define(string name, TypeInfo type)
        {
            throw new NotImplementedException();
        }

        public bool Define(string name, TypeInfo[] args, TypeInfo type)
        {
            throw new NotImplementedException();
        }

        public bool Change(string name, TypeInfo type)
        {
            throw new NotImplementedException();
        }

        public IScope CreateChild()
        {
            throw new NotImplementedException();
        }
    }
}
