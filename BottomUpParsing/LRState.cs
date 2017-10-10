using System.Collections.Generic;
using System.Linq;
using Grammars;

namespace BottomUpParsing
{
    public class LrState
    {
        public LrState(int number, Grammar g)
        {
            G = g;
            StateNumber = number;
            Items = new List<LrItem>();
            Gotos = new List<Goto>();
        }

        public Grammar G { get; }

        public int StateNumber { get; set; }
        public List<LrItem> Items { get; }
        public List<Goto>Gotos { get; set; }


        public virtual void SetClausure(ProductionAttr[] productions)
        {
            bool change;
            var temp = new List<LrItem>();
            do
            {
                foreach (var t in temp)
                    Items.Add(t);
                temp = new List<LrItem>();
                change = false;
                foreach (var i in Items)
                {
                    Production p = productions[i.ProductionNumber];
                    if (i.DotNumber >= p.Right.Length) continue;
                    if (p.Right[i.DotNumber] is NonTerminal)
                        for (var j = 0; j < productions.Length; j++)
                            if (productions[j].Left == p.Right[i.DotNumber])
                            {
                                var tmp = 0;
                                if (productions[j].IsEpsilon)
                                    tmp = 1;
                                var l = new LrItem(j, tmp);
                                if (Search(Items, l)) continue;
                                if (!Search(temp, l))
                                    temp.Add(l);
                                change = true;
                            }
                }
            } while (change);
        }


        public virtual bool Search(IEnumerable<LrItem> items, LrItem lrItem)
        {
            return items.Any(i => i.DotNumber == lrItem.DotNumber && i.ProductionNumber == lrItem.ProductionNumber);
        }
    }
}