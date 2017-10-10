using System;

namespace Lexer
{
    internal class Nfa
    {
        internal Nfa(char q, int level)
        {
            First = new State(level);
            Last = new State(level) {Final = true};
            First.AddLink(q, Last);
        }

        internal Nfa()
        {
        }

        public State First { get; private set; }
        public State Last { get; private set; }
        private int Level { get; set; }

        internal static Nfa Pow(Nfa p1, int level)
        {
            var ret = new Nfa
            {
                First = new State(level),
                Last = new State(level) {Final = true},
                Level = level
            };
            ret.First.AddLink('ε', p1.First);
            p1.Last.AddLink('ε', ret.Last);
            p1.Last.AddLink('ε', p1.First);
            p1.Last.Final = false;
            ret.First.AddLink('ε', ret.Last);
            return ret;
        }

        internal static Nfa NfaOrEps(Nfa p1, int level)
        {
            return Or(p1, new Nfa('ε', level), level);
        }

        internal static Nfa PowG1(Nfa p1, int level)
        {
            return Mul(p1, Pow(p1, level), level);
        }

        internal static Nfa MultiOr(char ini, char end, int level)
        {
            if (end < ini) throw new Exception();
            var ret = new Nfa(ini, level);
            for (var i = (char) (ini + 1); i <= end; i++)
                ret = Or(ret, new Nfa(i, level), level);
            return ret;
        }

        internal static Nfa Or(Nfa p1, Nfa p2, int level)
        {
            var ret = new Nfa
            {
                First = new State(level),
                Last = new State(level) {Final = true},
                Level = level
            };
            ret.First.AddLink('ε', p1.First);
            ret.First.AddLink('ε', p2.First);
            p1.Last.AddLink('ε', ret.Last);
            p2.Last.AddLink('ε', ret.Last);
            p1.Last.Final = false;
            p2.Last.Final = false;
            return ret;
        }

        internal static Nfa Mul(Nfa p1, Nfa p2, int level)
        {
            var ret = new Nfa {Level = level};
            p1.Last.Merge(p2.First);
            p2.First = p1.Last;
            p1.Last.Final = false;
            ret.First = p1.First;
            ret.Last = p2.Last;
            return ret;
        }
    }
}