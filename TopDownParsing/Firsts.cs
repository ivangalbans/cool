using System.Collections.Generic;
using System.Text;
using Grammars;

namespace TopDownParsing
{
    public class Firsts : Dictionary<Symbol, FirstSet>
    {
        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var pair in this)
                builder.Append($@"First( {pair.Key} ) = {pair.Value}");

            return builder.ToString();
        }

        public static Firsts Compute(Grammar G)
        {
            var firsts = new Firsts();

            foreach (var t in G.Terminals) firsts[t] = new FirstSet {t};
            foreach (var T in G.NonTerminals) firsts[T] = new FirstSet();

            firsts[G.EOF] = new FirstSet {G.EOF};
            firsts[G.Epsilon] = new FirstSet {G.Epsilon};
            bool changed;

            do
            {
                changed = false;

                foreach (var p in G.Productions)
                {
                    var X = p.Left;
                    var W = p.Right;

                    if (p.IsEpsilon)
                    {
                        changed |= firsts[X].Add(G.Epsilon);
                    }
                    else
                    {
                        var allEpsilon = true;

                        foreach (var s in W)
                        {
                            changed |= firsts[X].AddAll(firsts[s]);

                            if (!firsts[s].Contains(G.Epsilon))
                            {
                                allEpsilon = false;
                                break;
                            }
                        }

                        if (allEpsilon)
                            changed |= firsts[X].Add(G.Epsilon);
                    }
                }
            } while (changed);

            return firsts;
        }
    }
}