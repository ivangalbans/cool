using System.Collections.Generic;
using System.Linq;
using BottomUpParsing;
using Grammars;
using Parsing;
using ErrorLogger;

namespace Lexer
{
    internal class SimpleLexer : ILexer<Token>
    {
        public IEnumerable<Token> Lex(string a, Grammar g)
        {
            var types = new Dictionary<string, string>
            {
                {"|", "|"},
                {"+", "+"},
                {"*", "*"},
                {"?", "?"},
                {"[", "["},
                {"]", "]"},
                {"(", "("},
                {")", ")"},
                {"-", "-"},
                {"\\e", "epsilon"}
            };
            var text = "";
            for (var i = 0; i < a.Length; i++)
            {
                text += a[i];
                if (a[i] == '\\')
                    text += a[++i];
                if (!types.TryGetValue(text, out string type))
                    type = "symbol";
                yield return new Token(i, type == "epsilon" ? "symbol" : type,
                    type == "epsilon" ? "ε" : text[0] != '\\' ? text : text.Substring(1), i, 0);
                text = "";
            }
            yield return new Token(-2, "EOF", g.EOF.Name, 0, 0);
        }
    }

    public class LexerEngine : ILexer<Token>
    {
        private static readonly Grammar G;
        private static int _level;
        private readonly State _engine;
        private readonly Dictionary<int, string> _matchLevel;

        static LexerEngine()
        {
            G = new Grammar();
            var E = G.NonTerminal("E", true);
            var T = G.NonTerminal("T");
            var F = G.NonTerminal("F");
            var H = G.NonTerminal("H");
            var M = G.NonTerminal("M");
            var symbol = G.Terminal("symbol");
            var or = G.Terminal("|");
            var pow = G.Terminal("*");
            var plus = G.Terminal("+");
            var quest = G.Terminal("?");
            var open = G.Terminal("(");
            var closed = G.Terminal(")");
            var cop = G.Terminal("[");
            var ccl = G.Terminal("]");
            var less = G.Terminal("-");

            E %= (E + or + T).With(p => Nfa.Or(p[0], p[2], _level));
            E %= T.With(p => p[0]);
            T %= (T + F).With(p => Nfa.Mul(p[0], p[1], _level));
            T %= F.With(p => p[0]);
            F %= (H + pow).With(p => Nfa.Pow(p[0], _level));
            F %= (H + plus).With(p => Nfa.PowG1(p[0], _level));
            F %= (H + quest).With(p => Nfa.NfaOrEps(p[0], _level));
            F %= H.With(p => p[0]);
            H %= symbol.With(p => new Nfa(p[0][0], _level));
            H %= (open + E + closed).With(p => p[1]);
            H %= (cop + M + ccl).With(p => p[1]);
            M %= symbol.With(p => new Nfa(p[0][0], _level));
            M %= (symbol + less + symbol).With(p => Nfa.MultiOr(p[0][0], p[2][0], _level));
            M %= (symbol + M).With(p => Nfa.Or(new Nfa(p[0][0], _level), p[1], _level));
            M %= (symbol + less + symbol + M).With(p => Nfa.Or(Nfa.MultiOr(p[0][0], p[2][0], _level), p[3], _level));
        }

        public LexerEngine(IEnumerable<(string regex, string type)> regexPac)
        {
            var regexPacList = regexPac.ToList();
            regexPacList.Add((G.EOF.Name, "EOF"));
            _level = 0;
            var automaton = new Lr1Automaton(G);

            var table = new Lr1Table(automaton);
            _matchLevel = new Dictionary<int, string> {{-1, "ErrorToken"}, {-2, "EOF"}};
            _engine = new State(-1);
            var tokenTypeList = regexPacList.Select(x => x.type).ToList();
            foreach (var v in regexPacList.Select(x => x.regex))
            {
                table.TryParse(G, new SimpleLexer().Lex(v, G), out DerivationTree t);
                var nfa = t.Evaluate();
                _matchLevel.Add(_level, tokenTypeList[_level++]);
                _engine.AddLink('ε', nfa.First);
            }
        }

        public IEnumerable<Token> Lex(string str, Grammar g)
        {
            var tokens = Matcher.NfaSimulation(_engine, str).ToList();
            foreach (var id in tokens)
            {
                id.Type = _matchLevel[id.Id];
            }
            tokens.Add(new Token(-2, _matchLevel[-2], g.EOF.Name, 0, 0));
            return tokens;
        }
    }
}