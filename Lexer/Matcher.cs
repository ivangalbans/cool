using System.Collections.Generic;
using System.Linq;
using Grammars;

namespace Lexer
{
    public class Matcher
    {
        internal static IEnumerable<Token> NfaSimulation(State state, string str)
        {
            var s = new SortedSet<State>(new[] {state});
            Clousure(s);
            var tokenId = -1;
            var currentCounter = 0;
            var totalCounter = 0;
            var lastTokenPos = 0;
            for (var i = 0; i < str.Length;)
            {
                s = Move(s, str[i]);
                Clousure(s);
                currentCounter++;
                if (s.Count > 0)
                {
                    i++;
                    var finals = s.First();
                    if (!finals.Final) continue;
                    tokenId = finals.TokenPriority;
                    totalCounter = currentCounter;
                }
                else
                {
                    if (tokenId != -1)
                    {
                        yield return new Token(tokenId, "", str.Substring(lastTokenPos, totalCounter), 0, lastTokenPos);
                        tokenId = -1;
                        i = lastTokenPos + totalCounter;
                        lastTokenPos = i;
                        currentCounter = totalCounter = 0;
                    }
                    else
                    {
                        yield return new Token(tokenId, "", str.Substring(lastTokenPos, currentCounter), 0,
                            lastTokenPos);

                        i = lastTokenPos + currentCounter;
                        lastTokenPos = i;
                        currentCounter = totalCounter = 0;
                    }
                    s = new SortedSet<State>(new[] {state});
                    Clousure(s);
                }
            }
            yield return new Token(tokenId, "", str.Substring(lastTokenPos), 0, lastTokenPos);
        }

        private static SortedSet<State> Move(IEnumerable<State> s, char c)
        {
            var m = new SortedSet<State>();
            foreach (var t in s)
                m.UnionWith(t.GetLinks(c));
            return m;
        }

        private static void Clousure(ISet<State> s)
        {
            var stk = new Stack<State>(s);
            while (stk.Count > 0)
            {
                var t = stk.Pop();
                foreach (var u in t.GetLinks('ε').Where(u => !s.Contains(u)))
                {
                    s.Add(u);
                    stk.Push(u);
                }
            }
        }
    }
}