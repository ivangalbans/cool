using System;
using System.Collections;
using System.Collections.Generic;

namespace Grammars
{
    public class ProductionListAttr : IReadOnlyList<ProductionAttr>
    {
        private readonly List<ProductionAttr> _productions;

        internal ProductionListAttr()
        {
            _productions = new List<ProductionAttr>();
        }

        public IEnumerator<ProductionAttr> GetEnumerator()
        {
            return _productions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count => _productions.Count;
        public ProductionAttr this[int index] => _productions[index];

        internal void Add(ProductionAttr production)
        {
            if (production == null) throw new ArgumentNullException(nameof(production));
            _productions.Add(production);
        }
    }
}