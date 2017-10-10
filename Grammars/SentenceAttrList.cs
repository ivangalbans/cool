using System;
using System.Collections;
using System.Collections.Generic;

namespace Grammars
{
    public class SentenceAttrList : IEnumerable<SentenceAttr>
    {
        private readonly List<SentenceAttr> _sentences;

        public SentenceAttrList()
        {
            _sentences = new List<SentenceAttr>();
        }

        public IEnumerator<SentenceAttr> GetEnumerator()
        {
            return _sentences.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(SentenceAttr s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            _sentences.Add(s);
        }
    }
}