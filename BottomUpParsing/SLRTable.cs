using System;
using System.Collections.Generic;
using System.Linq;
using Error;
using Grammars;
using Parsing;
using TopDownParsing;

namespace BottomUpParsing
{
    public class SlrTable : IParser, IError
    {
        private readonly List<string> _errors;

        public SlrTable(LrAutomaton automaton)
        {
            _errors = new List<string>();
            Automaton = automaton;
            Table = new Dictionary<(int, Symbol), Actions>();
            if (ConflictInAutomaton())
            {
                MakeTable();
                Console.WriteLine("La Gramatica es LR(0)");
                this.ToString();
            }
            else
            {
                Console.WriteLine("La Gramatica no es LR(0)");
            }
        }

        private bool ConflictInAutomaton()
        {
            var fl = Follows.Compute(Automaton.G, Firsts.Compute(Automaton.G));
            var result = true;
            foreach (var state in Automaton.Automaton)
            {
                var reduce = Reduce(state);
                var shift = Shift(state);
                if(reduce.reult && shift.result)
                {
                    var Noterminal = Automaton.Productions[reduce.item.ProductionNumber].Left;
                   
                    foreach (var f in fl[Noterminal])
                    {
                        if(f.Name == shift.t.Name)
                        {
                            Console.WriteLine("Conflicto Shift-Reduce in State: " + state.StateNumber);
                            PrintState(state);
                            result = false;
                            break;
                        }
                    }                    
                }
                var reducereduce = ReduceReduce(state);
                if(reducereduce.result)
                {
                    foreach (var item in fl[Automaton.Productions[reducereduce.r1.ProductionNumber].Left])
                    {
                        if( foo(fl[Automaton.Productions[reducereduce.r2.ProductionNumber].Left] , item))
                        {
                            Console.WriteLine("Conflicto Reduce-Reduce in State: " + state.StateNumber);
                            PrintState(state);
                            result = false;
                            break;
                        }
                    }
                   
                }
            }
            return result;
        }

        private bool foo(FollowSet followSet, Terminal item)
        {
            
            foreach (var f in followSet)
            {
                if (f.Name == item.Name)
                    return true;
            }
            return false;
        }

        private void PrintState(LrState state)
        {
            Console.WriteLine("State" + state.StateNumber);
            foreach (var i in state.Items)
            {
                Console.Write(Automaton.Productions[i.ProductionNumber].Left + "-->");
                for (var j = 0; j < Automaton.Productions[i.ProductionNumber].Right.Length; j++)
                {
                    if (i.DotNumber == j) Console.Write(".");
                    Console.Write(Automaton.Productions[i.ProductionNumber].Right[j]);
                }
                if (i.DotNumber == Automaton.Productions[i.ProductionNumber].Right.Length) Console.Write(".");
                Console.Write(", ");
                Console.WriteLine();
            }
        }
        private (bool reult, LrItem item) Reduce(LrState l)
        {
            foreach (var x in l.Items)
            {
                if (Automaton.Productions[x.ProductionNumber].Right.Length == x.DotNumber)
                {
                    return (true, x);
                }
            }
            return (false, null);
        }

        private (bool result, Terminal t) Shift(LrState l)
        {
            foreach (var item in l.Items)
            {
                if (item.DotNumber == Automaton.Productions[item.ProductionNumber].Right.Length) continue;
                else
                {
                    Terminal a = Automaton.Productions[item.ProductionNumber].Right[item.DotNumber] as Terminal;
                    if (a != null)
                        return (true, a);
                }
            }
            return (false, null);
        }
        private (bool result, LrItem r1, LrItem r2) ReduceReduce(LrState l)
        {
            bool reduce = false;
            LrItem temp = null;
            for (int i = 0; i < l.Items.Count; i++)
            {
                var item = l.Items[i];

                if (Automaton.Productions[item.ProductionNumber].Right.Length == item.DotNumber)
                {
                    if (reduce) return (true, temp, item);
                    else { reduce = true; temp = item; }
                }
            }
            return (false, null, null);
        }
        public Dictionary<(int, Symbol), Actions> Table { get; }
        public LrAutomaton Automaton { get; }

        public Action<dynamic[]> Logger { get; set; }

        public bool TryParse(Grammar g, IEnumerable<Token> tokens, out DerivationTree tree)
        {
            var productions = Parser(g, tokens.ToList());

            if (_errors.Count == 0)
            {
                tree = DerivationTree.FromRightMost(productions);
                return true;
            }
            foreach (var errors in _errors)
                Console.WriteLine(errors);
            tree = null;
            return false;
        }

        public void MakeTable()
        {
            //_table = new Dictionary<(int, Symbol), Actions>();
            //FillTableDefualt(a);
            foreach (var states in Automaton.Automaton)
                foreach (var item in states.Items)
                {
                    if (item.ProductionNumber == 0 && item.DotNumber == 1)
                    {
                        (int, EOF) tuple = (states.StateNumber, Automaton.G.EOF);
                        var actions = new Actions
                        {
                            ActionType = ActionType.Accept,
                            ActionParameter = item.ProductionNumber
                        };
                        if (!Table.ContainsKey(tuple))
                            Table.Add(tuple, actions);
                        continue;
                    }

                    if (Automaton.Productions[item.ProductionNumber].Right[item.DotNumber] is Terminal)
                    {
                        var contain = IsGoto(Automaton.Gotos, states.StateNumber,
                            Automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                        if (contain < 0) continue;
                        var t =
                            (states.StateNumber, Automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                        var actions = new Actions
                        {
                            ActionType = ActionType.Shift,
                            ActionParameter = contain
                        };
                        if (!Table.ContainsKey(t))
                            Table.Add(t, actions);
                    }

                    else
                    {
                        var contain = IsGoto(Automaton.Gotos, states.StateNumber,
                            Automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                        if (contain < 0) continue;
                        var t =
                            (states.StateNumber, Automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                        var actions = new Actions
                        {
                            ActionType = ActionType.Goto,
                            ActionParameter = contain
                        };
                        if (!Table.ContainsKey(t))
                            Table.Add(t, actions);
                    }

                }

            
        }



        private void FillTableDefualt(List<LrState> a)
        {
            foreach (var s in a)
            {
                foreach (var symbol in Automaton.G.Symbols)
                {
                    (int,Symbol) t = (s.StateNumber, symbol);
                    Table[t] = new Actions {ActionType = ActionType.None, ActionParameter = -1};
                }
                (int,EOF) t1 = (s.StateNumber, Automaton.G.EOF);
                Table[t1] = new Actions {ActionType = ActionType.None, ActionParameter = -1};
            }
        }

        private static int IsGoto(IEnumerable<Goto> statesGotos, int x, Symbol symbol)
        {
            foreach (var g in statesGotos)
                if (g.NextToken == symbol && x == g.StateNumber) return g.NextState;
            return -1;
        }

        public override string ToString()
        {
            foreach (var t in Table)
                    Console.WriteLine("[" + t.Key.Item1 + "," + t.Key.Item2 + "] = " + t.Value.ActionType + " " +
                                      t.Value.ActionParameter);
            return "";
        }

        public IEnumerable<(ProductionAttr, List<Token>)> Parser(Grammar g, List<Token> tokens)
        {
            var result = new List<(ProductionAttr, List<Token>)>();
            var stack = new Stack<(int, Token)>();
            stack.Push((0, null));
            var pos = 0;
            while (true)
            {
                var s = stack.Peek();
                Symbol temp;
                Token tok;

                try
                {
                    temp = g.Terminals.First(x => x.Name == tokens[pos].Type);
                    tok = tokens[pos];
                }
                catch (Exception)
                {
                    if (tokens[pos].Type == "EOF")
                    {
                        temp = g.EOF;
                        tok = null;
                    }
                    else
                    {
                        Logger += p => ParsingError(tokens[pos++]);
                        continue;
                    }
                }
                var t = (s.Item1, temp);
                if (Table[t].ActionType == ActionType.Shift)
                {
                    stack.Push((Table[t].ActionParameter, tok));
                    pos++;
                }
                else
                {
                    if (Table[t].ActionType == ActionType.Reduce)
                    {
                        var len = Automaton.Productions[Table[t].ActionParameter].Right.Length;
                        var toks = new List<Token>();
                        while (len > 0)
                        {
                            var tmp = stack.Pop();
                            if (tmp.Item2 != null)
                                toks.Add(tmp.Item2);
                            len--;
                        }
                        var top = stack.Peek().Item1;
                        foreach (var state in Automaton.Automaton)
                        foreach (var go in state.Gotos)
                            if (go.StateNumber == state.StateNumber)
                                if (go.NextToken == Automaton.Productions[Table[t].ActionParameter].Left &&
                                    go.StateNumber == top)
                                    stack.Push((go.NextState, null));
                        toks.Reverse();
                        result.Add((Automaton.Productions[Table[t].ActionParameter], new List<Token>(toks)));
                        toks.Clear();
                    }
                    else
                    {
                        if (Table[t].ActionType == ActionType.Accept)
                        {
                            if (_errors.Count == 0)
                                Console.WriteLine("Accept");
                            return result;
                        }
                        Logger += p => ParsingError(tokens[pos++]);
                    }
                }
            }
        }

        public void ParsingError(Token token)
        {
            _errors.Add("Parsing Error in Line: " + token.Line + " Column: " + token.Column + " : " + token.Text +
                        " not match with grammar rules");
        }
    }
}