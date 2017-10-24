using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grammars;

namespace TopDownParsing
{
    public class Follows : Dictionary<NonTerminal, FollowSet>
    {
        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var pair in this) builder.Append($"Follow( {pair.Key} ) = {pair.Value}\n");

            return builder.ToString();
        }

        public static Follows Compute(Grammar G, Firsts firsts)
        {
            var Follows = new Follows();

            foreach (var X in G.NonTerminals)
                Follows[X] = new FollowSet();

            Follows[G.StartSymbol] = new FollowSet {G.EOF};

            bool changed;
            do
            {
                changed = false;

                foreach (var p in G.Productions)
                {
                    var X = p.Left;
                    var W = p.Right;

                    for (var i = 0; i < W.Length; i++)
                    {
                        var S = W[i] as NonTerminal;

                        if (S == null)
                            continue;

                        var first = FirstSet.Compute(G, W.Sufix(i + 1), firsts);
                        changed |= Follows[S].AddAll(first.Except(new[] {G.Epsilon}));

                        if (first.Contains(G.Epsilon) || i == W.Length - 1)
                            changed |= Follows[S].AddAll(Follows[X]);
                    }
                }
            } while (changed);

            return Follows;
        }
    }
}