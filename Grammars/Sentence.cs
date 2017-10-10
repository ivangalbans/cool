using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Grammars
{
    public class Sentence : IEnumerable<Symbol>
    {
        private readonly List<Symbol> _symbols;

        internal Sentence()
        {
            _symbols = new List<Symbol>();
        }

        public int Length => _symbols.Count;

        public Symbol this[int i] => _symbols[i];

        public IEnumerator<Symbol> GetEnumerator()
        {
            return _symbols.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Add(Symbol symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException(nameof(symbol));

            if (_symbols.Count == 1 && _symbols[0] == symbol.Grammar.Epsilon)
                throw new InvalidOperationException("A sentence cannot have Epsilon and other symbols");

            _symbols.Add(symbol);
        }


        public static SentenceList operator |(Sentence s1, Sentence s2)
        {
            return new SentenceList {s1, s2};
        }

        public static SentenceList operator |(SentenceList l, Sentence s)
        {
            l.Add(s);
            return l;
        }

        public static SentenceList operator |(Sentence sent, Symbol s)
        {
            return new SentenceList {sent, new Sentence {s}};
        }

        public static SentenceList operator |(Symbol s, Sentence sent)
        {
            return new SentenceList {new Sentence {s}, sent};
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var symbol in _symbols)
            {
                builder.Append(symbol);
                builder.Append(" ");
            }

            return builder.ToString();
        }

        public List<Symbol> Sufix(int i)
        {
            return _symbols.Skip(i).ToList();
        }
    }
}