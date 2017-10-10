using System;

namespace Grammars
{
    public static class Extensors
    {
        public static SentenceAttr With(this SentenceAttr n, Func<dynamic[], object> fun)
        {
            n.Evaluate = fun;
            return n;
        }

        public static SentenceAttr With(this Symbol n, Func<dynamic[], object> fun)
        {
            var s = new SentenceAttr {n};
            s.Evaluate = fun;
            return s;
        }
    }

    public class SentenceAttr : Sentence
    {
        public Func<dynamic[], dynamic> Evaluate { get; set; }
    }
}