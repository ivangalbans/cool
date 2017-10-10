using System;
using System.Collections.Generic;

namespace Lexer
{
    internal class State : IComparable<State>
    {
        public State(int tokenPriority)
        {
            Id = IdInc++;
            TokenPriority = tokenPriority;
            Final = false;
            Edges = new Dictionary<char, SortedSet<State>>();
        }

        private Dictionary<char, SortedSet<State>> Edges { get; set; }

        private int Id { get; }

        private static int IdInc { get; set; }

        public int TokenPriority { get; set; }

        public bool Final { get; set; }

        public int CompareTo(State other)
        {
            return Final && other.Final || !Final && !other.Final
                ? (TokenPriority == other.TokenPriority
                    ? Id - other.Id
                    : TokenPriority - other.TokenPriority)
                : (Final ? -1 : (other.Final ? 1 : 0));
        }

        public void AddLink(char q, State s)
        {
            if (!Edges.ContainsKey(q))
                Edges[q] = new SortedSet<State>();
            Edges[q].Add(s);
        }

        public void Merge(State q)
        {
            Edges = q.Edges;
        }

        public IEnumerable<State> GetLinks(char q)
        {
            return Edges.ContainsKey(q)
                ? Edges[q]
                : new SortedSet<State>();
        }
    }
}