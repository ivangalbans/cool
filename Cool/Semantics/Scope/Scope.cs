using Antlr4.Runtime;
using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    public class Scope : IScope
    {
        /// <summary>
        /// Information relative to variables.
        /// </summary>
        Dictionary<string, TypeInfo> _variables = new Dictionary<string, TypeInfo>();

        /// <summary>
        /// Information relative to variables.
        /// </summary>
        Dictionary<string, (TypeInfo[] Args, TypeInfo ReturnType)> _functions = new Dictionary<string, (TypeInfo[], TypeInfo)>();

        /// <summary>
        /// Information relative to types in the current scope.
        /// </summary>
        static Dictionary<string, TypeInfo> _declaredTypes = new Dictionary<string, TypeInfo>();

        public IScope Parent { get; set; } = nullScope;
        public TypeInfo Type { get; set; } = TypeInfo.NULL;

        static Scope()
        {
            _declaredTypes.Add("Object", TypeInfo.NULL);
            _declaredTypes.Add("Bool", new TypeInfo { Text = "Bool", Parent = _declaredTypes["Object"] });
            _declaredTypes.Add("Int", new TypeInfo { Text = "Int", Parent = _declaredTypes["Object"] });
            _declaredTypes.Add("String", new TypeInfo { Text = "String", Parent = _declaredTypes["Object"] });
            _declaredTypes.Add("IO", new TypeInfo { Text = "IO", Parent = _declaredTypes["Object"] });

            _declaredTypes["String"].ClassReference = new ClassNode(-1, -1, "String", "Object");
            _declaredTypes["String"].ClassReference.Scope.Define("length", new TypeInfo[0], _declaredTypes["Int"]);
            _declaredTypes["String"].ClassReference.Scope.Define("concat", new TypeInfo[1] { _declaredTypes["String"] }, _declaredTypes["String"]);
            _declaredTypes["String"].ClassReference.Scope.Define("substr", new TypeInfo[2] { _declaredTypes["Int"], _declaredTypes["Int"] }, _declaredTypes["String"]);
            
            _declaredTypes["Object"].ClassReference = new ClassNode(0, 0, "Object", "NULL");
            _declaredTypes["Object"].ClassReference.Scope.Define("abort", new TypeInfo[0], _declaredTypes["Object"]);
            _declaredTypes["Object"].ClassReference.Scope.Define("type_name", new TypeInfo[0], _declaredTypes["String"]);
            _declaredTypes["Object"].ClassReference.Scope.Define("copy", new TypeInfo[0], _declaredTypes["Object"]);

            _declaredTypes["IO"].ClassReference = new ClassNode(-1, -1, "IO", "Object");
            _declaredTypes["IO"].ClassReference.Scope.Define("out_string", new TypeInfo[1] { _declaredTypes["String"] }, _declaredTypes["String"]);
            _declaredTypes["IO"].ClassReference.Scope.Define("out_int", new TypeInfo[1] { _declaredTypes["Int"] }, _declaredTypes["Int"]);
            _declaredTypes["IO"].ClassReference.Scope.Define("in_string", new TypeInfo[0], _declaredTypes["String"]);
            _declaredTypes["IO"].ClassReference.Scope.Define("in_int", new TypeInfo[0], _declaredTypes["Int"]);
            
        }

        public static void Clear()
        {
            var tmp = new Dictionary<string, TypeInfo>();
            HashSet<string> builtin = new HashSet<string> { "Object", "Bool", "Int", "String", "IO" };
            foreach (var item in _declaredTypes)
                if(builtin.Contains(item.Key))
                    tmp.Add(item.Key, item.Value);
            _declaredTypes = tmp;
        }

        public bool IsDefined(string name, out TypeInfo type)
        {
            return _variables.TryGetValue(name, out type) ||
                    Parent.IsDefined(name, out type);
        }

        public bool IsDefined(string name, TypeInfo[] args, out TypeInfo type)
        {
            type = TypeInfo.ObjectType;
            if(_functions.ContainsKey(name) && _functions[name].Args.Length == args.Length)
            {
                bool ok = true;
                for (int i = 0; i < args.Length; ++i)
                    //the type of parameters must be equal each one.
                    if ((args[i] != _functions[name].Args[i]))
                        ok = false;
                if(ok)
                {
                    type = _functions[name].ReturnType;
                    return true;
                }
            }

            return Parent.IsDefined(name, args, out type) ||
                   Type.Parent.ClassReference.Scope.IsDefined(name, args, out type);
        }

        public bool IsDefinedType(string name, out TypeInfo type)
        {
            return _declaredTypes.TryGetValue(name, out type);
        }

        public bool Define(string name, TypeInfo type)
        {
            if (_variables.ContainsKey(name))
                return false;
            _variables.Add(name, type);
            return true;
        }

        public bool Define(string name, TypeInfo[] args, TypeInfo type)
        {
            if (_functions.ContainsKey(name))
                return false;
            _functions[name] = (args, type);
            return true;
        }

        public bool Change(string name, TypeInfo type)
        {
            if (!_variables.ContainsKey(name))
                return false;
            _variables[name] = type;
            return true;
        }

        public IScope CreateChild()
        {
            return new Scope()
            {
                Parent = this,
                Type = this.Type
            };
        }

        public bool AddType(string name, TypeInfo type)
        {
            _declaredTypes.Add(name, type);
            return true;
        }

        public TypeInfo GetType(string name)
        {
            if (_declaredTypes.TryGetValue(name, out TypeInfo type))
                return type;
            return TypeInfo.NULL;
        }

        #region
        private static NullScope nullScope = new NullScope();

        public static NullScope NULL => nullScope;

        public class NullScope : IScope
        {
            public IScope Parent { get; set; }
            public TypeInfo Type { get; set; } = TypeInfo.NULL;

            public bool AddType(string name, TypeInfo type)
            {
                return false;
            }

            public bool Change(string name, TypeInfo type)
            {
                return false;
            }

            public IScope CreateChild()
            {
                return new Scope()
                {
                    Parent = NULL,
                    Type = TypeInfo.NULL
                };
            }

            public bool Define(string name, TypeInfo type)
            {
                return false;
            }

            public bool Define(string name, TypeInfo[] args, TypeInfo type)
            {
                return false;
            }

            public TypeInfo GetType(string name)
            {
                return TypeInfo.NULL;
            }

            public bool IsDefined(string name, out TypeInfo type)
            {
                type = TypeInfo.ObjectType;
                return false;
            }

            public bool IsDefined(string name, TypeInfo[] args, out TypeInfo type)
            {
                type = TypeInfo.ObjectType;
                return false;
            }

            public bool IsDefinedType(string name, out TypeInfo type)
            {
                type = TypeInfo.ObjectType;
                return false;
            }
        }
        #endregion

    }
}
