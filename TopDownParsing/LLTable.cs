using System;
using System.Collections.Generic;
using System.Text;
using Grammars;

namespace TopDownParsing
{
    public class LlTable
    {
        private readonly Dictionary<(NonTerminal, Terminal), ProductionAttr> _table;

        private LlTable()
        {
            _table = new Dictionary<(NonTerminal, Terminal), ProductionAttr>();
        }

        public ProductionAttr this[NonTerminal symbol, Terminal terminal]
        {
            get => _table.TryGetValue((symbol, terminal), out ProductionAttr result) ? result : null;
            private set
            {
                if (value == null)
                    throw new ArgumentNullException();


                if (_table.TryGetValue((symbol, terminal), out ProductionAttr result) && result != value)
                    throw new InvalidOperationException("Entry already used.");

                _table[(symbol, terminal)] = value;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var pair in _table)
                builder.Append($"[{pair.Key.Item1}, {pair.Key.Item2}] = {pair.Value}\n");

            return builder.ToString();
        }

        public static LlTable Build(Grammar G)
        {
            var table = new LlTable();

            var firsts = Firsts.Compute(G);
            var follows = Follows.Compute(G, firsts);

            foreach (var p in G.Productions)
            {
                var X = p.Left;

                if (p.IsEpsilon) foreach (var t in follows[X]) table[X, t] = p;
                else foreach (var t in FirstSet.Compute(G, p.Right, firsts)) table[X, t] = p;
            }

            return table;
        }
    }
}