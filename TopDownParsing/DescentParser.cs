using System;
using System.Collections.Generic;
using System.Linq;
using Error;
using Grammars;
using Parsing;

namespace TopDownParsing
{
    public class DescentParser : IParser, IError
    {
        public Action<dynamic[]> Logger { get; set; }

        public bool TryParse(Grammar g, IEnumerable<Token> tokens, out DerivationTree tree)
        {
            try
            {
                var productions = LeftMostParse(g, tokens.ToArray());
                tree = DerivationTree.FromLeftMost(productions);
                return true;
            }
            catch (ParsingException)
            {
                tree = null;
                return false;
            }
        }

        private IEnumerable<ProductionAttr> LeftMostParse(Grammar G, params Token[] tokens)
        {
            var stack = new Stack<Symbol>(new Symbol[] {G.EOF, G.StartSymbol});
            var table = LlTable.Build(G);
            var nextToken = 0;

            while (stack.Count > 0 && nextToken < tokens.Length)
            {
                var symbol = stack.Pop();

                if (symbol is Terminal && tokens[nextToken++].Type != symbol.Name)
                    Logger += p => ParsingError(tokens[nextToken - 1]);

                if (symbol is NonTerminal)
                {
                    var production = table[symbol as NonTerminal,
                        G.Terminals.First(x => x.Name == tokens[nextToken].Type)];

                    if (production == null)
                        Logger += p => ParsingError(tokens[nextToken]);

                    yield return production;

                    if (production.IsEpsilon)
                        continue;

                    foreach (var s in production.Right.Reverse())
                        stack.Push(s);
                }
            }
        }

        public void ParsingError(Token token)
        {
            Console.WriteLine($"Parsing Error in Line: {token.Line} Column: {token.Column}");
            Console.WriteLine(token.Text + " not match with grammar rules");
        }
    }
}