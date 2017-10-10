using System;
using System.Collections;
using System.Collections.Generic;

namespace Grammars
{
    public class SentenceList : IEnumerable<Sentence>
    {
        private readonly List<Sentence> _sentences;

        public SentenceList()
        {
            _sentences = new List<Sentence>();
        }

        public IEnumerator<Sentence> GetEnumerator()
        {
            return _sentences.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Sentence s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            _sentences.Add(s);
        }
    }
}