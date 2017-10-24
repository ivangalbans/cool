using System.Collections.Generic;
using Grammars;

namespace TopDownParsing
{
    public class FirstSet : HashSet<Terminal>
    {
        public bool AddAll(IEnumerable<Terminal> firstSet)
        {
            var changed = false;

            foreach (var symbol in firstSet)
                changed |= Add(symbol);

            return changed;
        }

        public override string ToString()
        {
            return $"{{ {string.Join(", ", this)} }}";
        }

        public static FirstSet Compute(Grammar G, IEnumerable<Symbol> p, Firsts firsts)
        {
            var result = new FirstSet();
            var allEpsilon = true;

            foreach (var s in p)
            {
                result.AddAll(firsts[s]);

                if (!firsts[s].Contains(G.Epsilon))
                {
                    allEpsilon = false;
                    break;
                }
            }

            if (allEpsilon)
                result.Add(G.Epsilon);

            return result;
        }
    }
}