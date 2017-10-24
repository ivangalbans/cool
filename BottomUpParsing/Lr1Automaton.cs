using System;
using System.Collections.Generic;
using System.Linq;
using Grammars;

namespace BottomUpParsing
{
    public class Lr1Automaton
    {
        private Dictionary<NonTerminal, List<int>> _prodInfo;
        private List<int> _toMerge;

        public Lr1Automaton(Grammar g)
        {
            Inqueue = new Queue<Lr1State>();
            Currentstate = 0;
            G = g;
            Gotos = new List<Goto>();
            Automaton = new List<Lr1State>();
            Productions = new ProductionAttr[g.Productions.Count() + 1];
            FillStructuresToUse();
            MakeAutomaton();
        }

        public Grammar G { get; }
        internal List<Lr1State> Automaton { get; }
        private Queue<Lr1State> Inqueue { get; }
        internal List<Lr1State> LalrAutomaton { get; set; }
        internal List<Goto> Gotos { get; }
        internal ProductionAttr[] Productions { get; }
        private int Currentstate { get; set; }

        private void FillStructuresToUse()
        {
            _prodInfo = new Dictionary<NonTerminal, List<int>>();
            var numberproduction = 1;
            foreach (var p in G.Productions)
            {
                if (!_prodInfo.ContainsKey(p.Left))
                    _prodInfo.Add(p.Left, new List<int>());
                _prodInfo[p.Left].Add(numberproduction);
                Productions[numberproduction++] = p;
            }

            var s = G.NonTerminal("S'");
            s %= G.StartSymbol;

            Productions[0] = G.Productions[G.Productions.Count - 1];
            if (!_prodInfo.ContainsKey(G.Productions[G.Productions.Count - 1].Left))
                _prodInfo.Add(G.Productions[G.Productions.Count - 1].Left, new List<int>());
            _prodInfo[G.Productions[G.Productions.Count - 1].Left].Add(0);
        }

        private void MakeAutomaton()
        {
            var lr1 = new Lr1Item(0, 0, new List<Symbol> {G.EOF});
            var s = new Lr1State(0, G);

            s.Add(lr1);
            s.SetClosure(Productions, _prodInfo);

            JoinLookhead(s);
            Inqueue.Enqueue(s);
            //var res = 0;
            while (Inqueue.Count > 0)
            {
                var temp = Inqueue.Dequeue();
                Automaton.Add(temp);
                MakeGoto(temp);
            }
        }

        private void MakeGoto(Lr1State temp)
        {
            var dict = new Dictionary<Symbol, List<Lr1Item>>();

            foreach (var item in temp.Items)
            {
                if (item.DotNumber >= Productions[item.ProductionNumber].Right.Length)
                    continue;

                var tmp = Productions[item.ProductionNumber].Right[item.DotNumber];
                if (!dict.ContainsKey(tmp))
                    dict.Add(tmp, new List<Lr1Item>());
                dict[tmp].Add(new Lr1Item(item.ProductionNumber, item.DotNumber + 1, item.Lookahead));
            }

            foreach (var keyValue in dict)
            {
                var listtemp = new List<Lr1Item>();
                listtemp = keyValue.Value;
                var lr1State = Search2((listtemp, keyValue.Key));

                if (lr1State == null)
                {
                    lr1State = new Lr1State(++Currentstate, G);
                    lr1State.AddRange(listtemp);
                    lr1State.SetClosure(Productions, _prodInfo);

                    JoinLookhead(lr1State);
                    Inqueue.Enqueue(lr1State);
                }
                var g1 = new Goto(temp.StateNumber, keyValue.Key,
                    lr1State.StateNumber);
                if (!NotIn(g1, Gotos))
                    Gotos.Add(g1);
            }
        }

        //NEW me busca si esta lista de items esta en algun estado del automata. Si lo esta adiciona los lk
        private Lr1State Search2((List<Lr1Item> l, Symbol match) tuple)
        {
            Lr1State result = null;
            foreach (var state in Automaton)
            {
                result = state;

                if (!FindInAutomaton(tuple.l, state.Items, tuple.match))
                    result = null;
                if (result != null)
                    return result;
            }

            foreach (var state in Inqueue)
            {
                result = state;
                if (!FindInAutomaton(tuple.l, state.Items, tuple.match))
                    result = null;
                if (result != null)
                    return result;
            }
            return null;
        }

        private bool FindInAutomaton(List<Lr1Item> p0, List<Lr1Item> stateItems, Symbol s)
        {
            if (Count(stateItems, s) != p0.Count) return false;
            var c = 0;
            foreach (var i in p0)
            foreach (var j in stateItems)
                if (i.DotNumber == j.DotNumber && i.ProductionNumber == j.ProductionNumber &&
                    Lr1State.EqualsLookahead(i.Lookahead, j.Lookahead))
                {
                    c++;
                    break;
                }
            return c == p0.Count;
        }

        private int Count(List<Lr1Item> stateItems, Symbol symbol)
        {
            var result = 0;
            foreach (var s in stateItems)
            {
                if (s.DotNumber == 0) continue;
                if (Productions[s.ProductionNumber].Right[s.DotNumber - 1] == symbol)
                    result++;
            }
            return result;
        }

        private void JoinLookhead(Lr1State got)
        {
            var dict = new Dictionary<(int, int), List<Symbol>>();
            var a = new List<Lr1Item>();
            var aset = new HashSet<int>();

            foreach (var i in got.Items)
            {
                if (!dict.ContainsKey((i.DotNumber, i.ProductionNumber)))
                    dict.Add((i.DotNumber, i.ProductionNumber), new List<Symbol>());
                dict[(i.DotNumber, i.ProductionNumber)].AddRange(i.Lookahead);
            }
            foreach (var v in dict)
            {
                var d = new Lr1Item(v.Key.Item2, v.Key.Item1, v.Value);
                a.Add(d);
                aset.Add(d.GetHashCode());
            }

            got.Items = a;
            got.ItemSet = aset;
        }

        private static bool NotIn(Goto @goto, List<Goto> gotos)
        {
            return gotos.Any(g => g.StateNumber == @goto.StateNumber && g.NextState == @goto.NextState &&
                                  g.NextToken == @goto.NextToken);
        }

        public override string ToString()
        {
            foreach (var state in Automaton)
            {
                Console.WriteLine("State" + state.StateNumber);
                foreach (var i in state.Items)
                {
                    Console.Write(Productions[i.ProductionNumber].Left + "-->");
                    for (var j = 0; j < Productions[i.ProductionNumber].Right.Length; j++)
                    {
                        if (i.DotNumber == j)
                            Console.Write(".");
                        Console.Write(Productions[i.ProductionNumber].Right[j]);
                    }

                    if (i.DotNumber == Productions[i.ProductionNumber].Right.Length)
                        Console.Write(".");
                    Console.Write(", ");

                    foreach (var l in i.Lookahead)
                        Console.Write(l + "|");
                    Console.WriteLine();
                }

                foreach (var g in Gotos)
                    if (g.StateNumber == state.StateNumber)
                        Console.WriteLine("Goto(" + g.StateNumber + "," + g.NextToken + ") = " + g.NextState);

                Console.WriteLine();
                Console.WriteLine();
            }
            return "";
        }

        public void ConvertToLalr()
        {
            LalrAutomaton = new List<Lr1State>();
            _toMerge = new List<int>();

            foreach (var temp in Automaton)
            {
                var used = false;

                foreach (var y in _toMerge)
                    if (y == temp.StateNumber)
                        used = true;

                if (used) continue;
                SearchToMerge(temp);
            }

            Build();
        }

        private void Build()
        {
            foreach (var lr1State in Automaton)
                if (!_toMerge.Contains(lr1State.StateNumber))
                    LalrAutomaton.Add(lr1State);
            PrintLalr();
        }

        private void PrintLalr()
        {
            Console.WriteLine("LALR Automaton");
            foreach (var state in LalrAutomaton)
            {
                Console.WriteLine("State" + state.StateNumber);
                foreach (var i in state.Items)
                {
                    Console.Write(Productions[i.ProductionNumber].Left + "-->");
                    for (var j = 0; j < Productions[i.ProductionNumber].Right.Length; j++)
                    {
                        if (i.DotNumber == j)
                            Console.Write(".");
                        Console.Write(Productions[i.ProductionNumber].Right[j]);
                    }

                    if (i.DotNumber == Productions[i.ProductionNumber].Right.Length)
                        Console.Write(".");
                    Console.Write(", ");

                    foreach (var l in i.Lookahead)
                        Console.Write(l + "|");
                    Console.WriteLine();
                }
                foreach (var g in Gotos)
                    if (g.StateNumber == state.StateNumber)
                        Console.WriteLine("Goto(" + g.StateNumber + "," + g.NextToken + ") = " + g.NextState);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        private void SearchToMerge(Lr1State temp)
        {
            foreach (var a in Automaton)
                if (a.StateNumber != temp.StateNumber)
                    if (a.Items.Count == temp.Items.Count)
                    {
                        var used = false;
                        var aux = new List<int>();
                        for (var i = 0; i < a.Items.Count; i++)
                        {
                            if (a.Items[i].ProductionNumber == temp.Items[i].ProductionNumber &&
                                a.Items[i].DotNumber == temp.Items[i].DotNumber)
                            {
                                if (!aux.Contains(a.StateNumber))
                                    aux.Add(a.StateNumber);
                            }
                            else
                            {
                                used = true;
                            }
                            if (used) break;
                        }
                        if (!used)
                        {
                            _toMerge.AddRange(aux);

                            for (var j = 0; j < a.Items.Count; j++)
                            {
                                temp.Items[j].Lookahead.AddRange(a.Items[j].Lookahead);
                                foreach (var g in Gotos)
                                {
                                    if (g.StateNumber == a.StateNumber) g.StateNumber = temp.StateNumber;
                                    if (g.NextState == a.StateNumber) g.NextState = temp.StateNumber;
                                }
                            }
                        }
                    }
        }
    }
}