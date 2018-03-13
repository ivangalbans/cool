using Cool.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool.Semantics.Algorithm
{
    static class Algorithm
    {
        enum Color { White, Gray, Black};

        static private Dictionary<string, int> _id;
        static private Color[] _mk;
        static List<int>[] g;
        static List<int> tp;
        static bool TopologicalSort(List<ClassNode> classNodes)
        {
            _id = new Dictionary<string, int>();
            _mk = new Color[classNodes.Count];
            tp = new List<int>();
            g = new List<int>[classNodes.Count];

            for (int i = 0; i < classNodes.Count; ++i)
                if (_id.ContainsKey(classNodes[i].TypeClass.TypeId))
                    return false;
                else _id.Add(classNodes[i].TypeClass.TypeId, i);

            foreach (var item in classNodes)
                g[Hash(item.TypeClass)].Add(Hash(item.TypeInherit));

            for (int u = 0; u < classNodes.Count; ++u)
                if (_mk[u] == Color.White && !Dfs(u))
                    return false;

            List<ClassNode> ans = new List<ClassNode>();
            foreach (var item in tp)
                ans.Add(classNodes[item]);

            classNodes = ans;
            return true;
        }

        private static bool Dfs(int u)
        {
            if (u == -1)
                return true;
            _mk[u] = Color.Gray;
            foreach (var v in g[u])
            {
                if (_mk[v] == Color.Gray)
                    return false;
                if (_mk[u] == Color.White && !Dfs(v))
                    return false;
            }
            tp.Add(u);
            _mk[u] = Color.Black;
            return true;
        }

        private static int Hash(TypeNode type)
        {
            return _id.ContainsKey(type.TypeId) ? _id[type.TypeId] : -1;
        }

    }
}
