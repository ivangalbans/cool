using System.Collections.Generic;
using Grammars;
using TopDownParsing;

namespace BottomUpParsing
{
    public class Lr1State : LrState
    {
        public Lr1State(int number, Grammar g) : base(number, g)
        {
            Items = new List<Lr1Item>();
            ItemSet = new HashSet<int>();
        }

        public new List<Lr1Item> Items { get; set; }
        public HashSet<int> ItemSet { get; set; }

        public void Add(Lr1Item lr1Item)
        {
            if (!ItemSet.Contains(lr1Item.GetHashCode()))
            {
                ItemSet.Add(lr1Item.GetHashCode());
                Items.Add(lr1Item);
            }
        }

        public void AddRange(List<Lr1Item> lr1Items)
        {
            foreach (var lr1Item in lr1Items)
                Add(lr1Item);
        }

        public void SetClosure(ProductionAttr[] productions, Dictionary<NonTerminal, List<int>>productionsInfo)
        {
            bool change;
            var temp = new List<Lr1Item>();
            do
            {
                Items.AddRange(temp);
                temp = new List<Lr1Item>();
                change = false;
                foreach (var i in Items)
                {
                    Production p = productions[i.ProductionNumber];
                    if (i.DotNumber >= p.Right.Length) continue;
                    if (!(p.Right[i.DotNumber] is NonTerminal nonTerminal)) continue;
                    for (var j = 0; j < productionsInfo[nonTerminal].Count; j++)
                    {
                        var k = productionsInfo[nonTerminal][j];
                        var lr1 = new Lr1Item(k, productions[k].IsEpsilon ? 1 : 0, new List<Symbol>());
                        var a = p.Right.Sufix(i.DotNumber + 1);

                        foreach (var l in i.Lookahead)
                        {
                            a.Add(l);
                            var tempp = FirstSet.Compute(G, a, Firsts.Compute(G));
                            a.RemoveAt(a.Count - 1);
                            foreach (var b in tempp)
                                if (!lr1.LookAheadSet.Contains(b) && b != G.Epsilon)
                                {
                                    lr1.LookAheadSet.Add(b);
                                    lr1.Lookahead.Add(b);
                                }
                        }

                        if (ItemSet.Contains(lr1.GetHashCode())) continue;
                        temp.Add(lr1);
                        ItemSet.Add(lr1.GetHashCode());
                        change = true;
                    }
                }
            } while (change);
        }

        public static bool EqualsLookahead(List<Symbol> iLookahead, List<Symbol> lrItemLookahead)
        {
            if (iLookahead.Count != lrItemLookahead.Count) return false;
            for (var i = 0; i < iLookahead.Count; i++)
                if (iLookahead[i].GetHashCode() != lrItemLookahead[i].GetHashCode())
                    return false;
            return true;
        }
    }
}