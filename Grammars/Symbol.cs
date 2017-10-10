using System.Linq;

namespace Grammars
{
    public abstract class Symbol
    {
        private bool change;

        private int mem;

        protected Symbol(string name, Grammar grammar)
        {
            Name = name;
            Grammar = grammar;
        }

        public string Name { get; }
        public Grammar Grammar { get; }

        public override int GetHashCode()
        {
            if (change) return mem;
            mem = Name.GetHashCode() * 977 +
                  Name.Aggregate(0, (current, x) => current + x.GetHashCode() + current * 977) * 613;
            change = true;
            return mem;
        }

        public override string ToString()
        {
            return Name;
        }

        public static SentenceAttr operator +(Symbol s1, Symbol s2)
        {
            return new SentenceAttr {s1, s2};
        }

        public static SentenceAttr operator +(SentenceAttr sent, Symbol s)
        {
            sent.Add(s);
            return sent;
        }
    }
}