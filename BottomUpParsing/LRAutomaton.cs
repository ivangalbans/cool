using System;
using System.Collections.Generic;
using System.Linq;
using Grammars;

namespace BottomUpParsing
{
    public class LrAutomaton
    {
        private readonly Queue<LrState> _currentStates;
        public List<Goto> Gotos { get; set; }
        private readonly Dictionary<int, LrState> _stateNumbers;
        private int _currentState;

        public LrAutomaton(Grammar g)
        {
            _stateNumbers = new Dictionary<int, LrState>();
            _currentState = 0;
            G = g;
            Productions = new ProductionAttr[G.Productions.Count() + 1];
            Automaton = new List<LrState>();
            Kernels = new Dictionary<int, List<LrItem>>();
            _currentStates = new Queue<LrState>();
            FillStructuresToUse();
            MakeAutomaton();
            Gotos = new List<Goto>();
            foreach (var item in Automaton)
            {
                Gotos.AddRange(item.Gotos);
            }
        }

        internal Grammar G { get; }
        internal List<LrState> Automaton { get; }
        internal ProductionAttr[] Productions { get; }
        private Dictionary<int, List<LrItem>> Kernels { get; }

        public void MakeAutomaton()
        {
            var s = new LrState(0, G);
            s.Items.Add(new LrItem(0, 0));
            s.SetClausure(Productions);
            _currentStates.Enqueue(s);
            var res = s.Items.Aggregate(0, (current, c) => current + (current * 311 + c.GetHashCode()) % 1000000009);
            _stateNumbers.Add(res, s);
            while (_currentStates.Count > 0)
            {
                var s1 = _currentStates.Dequeue();
                Automaton.Add(s1);
                MakeGoto(s1);
            }
            MakeKernels();
        }

        private void MakeKernels()
        {
            for (var index = 0; index < Automaton.Count; index++)
                Kernels.Add(index, new List<LrItem>());
            foreach (var state in Automaton)
                foreach (var item in state.Items)
                    if (state.StateNumber == 0 && item.ProductionNumber == 0)
                    {
                        Kernels[0].Add(item);
                    }
                    else
                    {
                        if (item.DotNumber > 0)
                            Kernels[state.StateNumber].Add(item);
                    }
        }

        private void MakeGoto(LrState state)
        {
            var dict = new Dictionary<Symbol, List<LrItem>>();
            foreach (var item in state.Items)
            {
                if (item.DotNumber >= Productions[item.ProductionNumber].Right.Length)
                    continue;

                var tmp = Productions[item.ProductionNumber].Right[item.DotNumber];
                if (!dict.ContainsKey(tmp))
                    dict.Add(tmp, new List<LrItem>());
                dict[tmp].Add(new LrItem(item.ProductionNumber, item.DotNumber + 1));
            }

            foreach (var listtemp in dict)
            {
                var t = new LrState(_currentState + 1, G);
                t.Items.AddRange(listtemp.Value);
                t.SetClausure(Productions);
                var res = t.Items.Aggregate(0,
                    (current, c) => current + (current * 311 + c.GetHashCode()) % 1000000009);
                if (_stateNumbers.ContainsKey(res))
                {
                    t = _stateNumbers[res];
                }
                else
                {
                    _currentState++;
                    _stateNumbers.Add(res, t);
                    _currentStates.Enqueue(t);
                }
                var g1 = new Goto(state.StateNumber, listtemp.Key,
                    t.StateNumber);
                if (!NotIn(state.Gotos, g1))
                    state.Gotos.Add(g1);
            }

        }

        private static bool NotIn(List<Goto> stateGotos, Goto @goto)
        {
            return stateGotos.Any(g => g.StateNumber == @goto.StateNumber && g.NextState == @goto.NextState &&
                                       g.NextToken == @goto.NextToken);
        }


        private void FillStructuresToUse()
        {
            var numberproduction = 1;
            foreach (var p in G.Productions)
                Productions[numberproduction++] = p;
            var s = G.NonTerminal("S'");
            s %= G.StartSymbol;


            Productions[0] = G.Productions.Last();
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
                        if (i.DotNumber == j) Console.Write(".");
                        Console.Write(Productions[i.ProductionNumber].Right[j]);
                    }
                    if (i.DotNumber == Productions[i.ProductionNumber].Right.Length) Console.Write(".");

                    Console.WriteLine();
                }
                foreach (var g in state.Gotos)
                    Console.WriteLine("Goto(" + g.StateNumber + "," + g.NextToken + ") = " + g.NextState);
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.WriteLine("Kernels");
            foreach (var k in Kernels)
            {
                Console.WriteLine("Kernel " + k.Key + "");
                foreach (var i in k.Value)
                {
                    Console.Write(Productions[i.ProductionNumber].Left + "-->");
                    for (var j = 0; j < Productions[i.ProductionNumber].Right.Length; j++)
                    {
                        if (i.DotNumber == j) Console.Write(".");
                        Console.Write(Productions[i.ProductionNumber].Right[j]);
                    }
                    if (i.DotNumber == Productions[i.ProductionNumber].Right.Length) Console.Write("*");
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            return "";
        }
    }
}