using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics
{
    public static class Algorithm
    {
        enum Color { White, Gray, Black};

        static private Dictionary<string, int> _id;
        static private Color[] _mk;
        static List<int>[] g;
        static List<int> tp;

        private static int idA, idB;

        private static void Init(int n)
        {
            _id = new Dictionary<string, int>();
            _mk = new Color[n];
            tp = new List<int>();
            g = new List<int>[n];
            for (int i = 0; i < n; ++i)
                g[i] = new List<int>();
        }

        public static bool TopologicalSort(List<ClassNode> classNodes, ICollection<SemanticError> errors)
        {
            int n = classNodes.Count;
            Init(n);
            
            for (int i = 0; i < n; ++i)
            {
                if(_id.ContainsKey(classNodes[i].TypeClass.Text))
                {
                    errors.Add(SemanticError.RepeatedClass(classNodes[i].TypeClass));
                    return false;
                }
                else
                {
                    _id.Add(classNodes[i].TypeClass.Text, i);
                }
            }

            foreach (var item in classNodes)
            {
                int u = Hash(item.TypeClass);
                int v = Hash(item.TypeInherit);
                if (v != -1)
                    g[u].Add(v);
            }

            for (int u = 0; u < n; ++u)
            {
                if (_mk[u] == Color.White && !Dfs(u))
                {
                    errors.Add(SemanticError.InvalidClassDependency(classNodes[idA].TypeClass, classNodes[idB].TypeClass));
                    return false;
                }
            }

            List<ClassNode> ans = new List<ClassNode>();
            foreach (var item in tp)
                ans.Add(classNodes[item]);

            classNodes = ans;
            return true;
        }

        private static bool Dfs(int u)
        {
            _mk[u] = Color.Gray;
            foreach (var v in g[u])
            {
                if (_mk[v] == Color.Gray)
                {
                    idA = u;
                    idB = v;
                    return false;
                }
                if (_mk[v] == Color.White && !Dfs(v))
                    return false;
            }
            tp.Add(u);
            _mk[u] = Color.Black;
            return true;
        }

        private static int Hash(TypeNode type)
        {
            return _id.ContainsKey(type.Text) ? _id[type.Text] : -1;
        }

        public static TypeInfo LowerCommonAncestor(TypeInfo type1, TypeInfo type2)
        {
            while (type1.Level < type2.Level)   type2 = type2.Parent;
            while (type2.Level < type1.Level)   type1 = type1.Parent;
            while(type1 != type2) { type1 = type1.Parent; type2 = type2.Parent; }
            return type1;
        }
    }
}
