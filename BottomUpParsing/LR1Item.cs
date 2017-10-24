using System.Collections.Generic;
using Grammars;

namespace BottomUpParsing
{
    public class Lr1Item : LrItem
    {
        private bool change;
        private int mem;

        public Lr1Item(int production, int dot, List<Symbol> lk) : base(production, dot)
        {
            Lookahead = lk;
            LookAheadSet = new HashSet<Symbol>(lk);
        }

        public List<Symbol> Lookahead { get; }
        public HashSet<Symbol> LookAheadSet { get; }

        public override int GetHashCode()
        {
            if (change) return mem;
            var res = 0;

            foreach (var symbol in Lookahead)
                res = (res + symbol.GetHashCode() * 311) % 1000000009;

            mem = (ProductionNumber * 503 + DotNumber + res) % 1000000009;
            change = true;

            return mem;
        }
    }
}