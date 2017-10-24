using System.Collections.Generic;

namespace Grammars
{
    public class NonTerminal : Symbol
    {
        private readonly ProductionList _productions = new ProductionList();

        public NonTerminal(string name, Grammar grammar)
            : base(name, grammar)
        {
        }

        public IEnumerable<Production> Productions => _productions;

        public static NonTerminal operator %(NonTerminal n, SentenceAttr s)
        {
            var production = new ProductionAttr(n, s);
            n._productions.Add(production);
            n.Grammar.Productions.Add(production);
            return n;
        }

        public static NonTerminal operator %(NonTerminal n, Symbol s)
        {
            var production = new ProductionAttr(n, new SentenceAttr {s});
            n._productions.Add(production);
            n.Grammar.Productions.Add(production);
            return n;
        }

        public static NonTerminal operator %(NonTerminal n, SentenceAttrList sent)
        {
            foreach (var s in sent)
            {
                var production = new ProductionAttr(n, s);
                n._productions.Add(production);
                n.Grammar.Productions.Add(production);
            }
            return n;
        }
    }
}