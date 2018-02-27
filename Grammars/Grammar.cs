using System;
using System.Collections.Generic;
using System.Text;

namespace Grammars
{
    public class Grammar
    {
        private readonly List<NonTerminal> _nonTerminals;
        private readonly List<Terminal> _terminals;

        public Grammar()
        {
            Productions = new ProductionListAttr();
            _nonTerminals = new List<NonTerminal>();
            _terminals = new List<Terminal>();

            Epsilon = new Epsilon(this);
            EOF = new EOF(this);
        }

        public IEnumerable<NonTerminal> NonTerminals => _nonTerminals;

        public IEnumerable<Terminal> Terminals => _terminals;

        public IEnumerable<Symbol> Symbols
        {
            get
            {
                foreach (var nonTerminal in NonTerminals) yield return nonTerminal;
                foreach (var terminal in Terminals) yield return terminal;
                yield return Epsilon;
            }
        }

        public ProductionListAttr Productions { get; }

        public NonTerminal StartSymbol { get; private set; }
        public Epsilon Epsilon { get; }
        public EOF EOF { get; }

        public NonTerminal NonTerminal(string name, bool startSymbol = false)
        {
            var term = new NonTerminal(name, this);

            if (startSymbol)
            {
                if (StartSymbol != null)
                    throw new InvalidOperationException("Cannot define more than one start symbol.");

                StartSymbol = term;
            }

            _nonTerminals.Add(term);
            return term;
        }

        public Terminal Terminal(string name)
        {
            var term = new Terminal(name, this);
            _terminals.Add(term);
            return term;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append("Non-Terminals: ");

            foreach (var terminal in NonTerminals)
            {
                builder.Append(terminal);
                builder.Append(", ");
            }

            builder.Append("\nTerminals: ");

            foreach (var terminal in Terminals)
            {
                builder.Append(terminal);
                builder.Append(", ");
            }

            builder.Append("\nProductions:\n");

            foreach (var production in Productions) builder.AppendLine(production.ToString());

            return builder.ToString();
        }
    }
}