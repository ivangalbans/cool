using System;
using System.Collections;
using System.Collections.Generic;

namespace Grammars
{
    public class ProductionList : IEnumerable<Production>
    {
        private readonly List<Production> _productions;

        internal ProductionList()
        {
            _productions = new List<Production>();
        }

        public IEnumerator<Production> GetEnumerator()
        {
            return _productions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Add(Production production)
        {
            if (production == null) throw new ArgumentNullException(nameof(production));
            _productions.Add(production);
        }
    }
}