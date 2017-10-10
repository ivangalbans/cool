using System;

namespace Grammars
{
    public class ProductionAttr : Production
    {
        public ProductionAttr(NonTerminal nonTerminal, SentenceAttr sentence) : base(nonTerminal, sentence)
        {
            Evaluate = sentence.Evaluate;
        }

        public Func<dynamic[], dynamic> Evaluate { get; set; }
    }
}