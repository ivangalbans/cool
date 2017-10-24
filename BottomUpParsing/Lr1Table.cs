using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Error;
using Grammars;
using Parsing;
using ErrorLogger;
namespace BottomUpParsing
{
    class Comp : IEqualityComparer<(int, Symbol)>
    {
        public bool Equals((int, Symbol) x, (int, Symbol) y)
        {
            return x.Item1 == y.Item1 && x.Item2.GetHashCode() == y.Item2.GetHashCode();
        }

        public int GetHashCode((int, Symbol) obj)
        {
            return obj.Item1.GetHashCode() * 919 + obj.Item2.GetHashCode();
        }
    }

    public class Lr1Table : IParser, IError
    {
        public Lr1Automaton _automaton;
        public Dictionary<(int, Symbol), Actions> _table;

        
        public Lr1Table(Lr1Automaton automaton)
        {
            _automaton = automaton;
            _table = new Dictionary<(int, Symbol), Actions>(new Comp());
            if (ConflictInAutomaton())
            {
                MakeTable(automaton.Automaton);
                Console.WriteLine("The grammar is LR(1)");
                this.ToString();
            }
            else
            {
                Console.WriteLine("The grammar is not LR(1)");
            }

        }
        private bool ConflictInAutomaton()
        {
            var result = true;
            foreach (var state in _automaton.Automaton)
            {
                var reduce = Reduce(state);
                var shift = Shift(state);
                if (reduce.reult && shift.result)
                {
                    if (ExistIn(reduce.item.Lookahead, shift.t))
                    {
                        Console.WriteLine("Conflicto Shift-Reduce in State: " + state.StateNumber);
                        PrintState(state);
                        result = false;
                        break;
                    }
                }
                var reducereduce = ReduceReduce(state);
                if (reducereduce.result)
                {
                    if (EqualsLookahead(reducereduce.r1.Lookahead, reducereduce.r2.Lookahead))
                    {
                        Console.WriteLine("Conflicto Reduce-Reduce in State: " + state.StateNumber);
                        PrintState(state);
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        private bool ExistIn(List<Symbol> l, Terminal t)
        {
            return l.Exists(x => x.Name == t.Name);
        }
        private bool EqualsLookahead(List<Symbol> l1 , List<Symbol> l2)
        {
            if (l1.Count == l2.Count)
            {
                foreach (var item in l1)
                {
                    if (!l2.Exists(x => x.Name == item.Name)) return false;
                }
                return true;
            }
            else return false;
        }
        private void PrintState(Lr1State state)
        {
            Console.WriteLine("State" + state.StateNumber);
            foreach (var i in state.Items)
            {
                Console.Write(_automaton.Productions[i.ProductionNumber].Left + "-->");
                for (var j = 0; j < _automaton.Productions[i.ProductionNumber].Right.Length; j++)
                {
                    if (i.DotNumber == j) Console.Write(".");
                    Console.Write(_automaton.Productions[i.ProductionNumber].Right[j]);
                }
                if (i.DotNumber == _automaton.Productions[i.ProductionNumber].Right.Length) Console.Write(".");
                Console.Write(", ");
                foreach (var l in i.Lookahead)
                    Console.Write(l + "|");
                Console.WriteLine();
            }
        }

        private (bool reult,Lr1Item item) Reduce(Lr1State l)
        {
            foreach (var x in l.Items)
            {
                if(_automaton.Productions[x.ProductionNumber].Right.Length == x.DotNumber)
                {
                    return (true, x);
                }
            }
            return (false, null);
        }

        private (bool result,Terminal t) Shift(Lr1State l)
        {
            foreach (var item in l.Items)
            {
                if (item.DotNumber == _automaton.Productions[item.ProductionNumber].Right.Length) continue;
                else
                {
                    Terminal a = _automaton.Productions[item.ProductionNumber].Right[item.DotNumber] as Terminal;
                    if (a != null)
                        return (true,a);
                }
            }
            return (false,null);
        }
        private (bool result,Lr1Item r1,Lr1Item r2) ReduceReduce(Lr1State l)
        {
            bool reduce = false;
            Lr1Item temp = null;
            for (int i = 0; i < l.Items.Count; i++)
            {
                var item = l.Items[i];

                if (_automaton.Productions[item.ProductionNumber].Right.Length == item.DotNumber)
                {
                    if (reduce) return (true,temp,item);
                    else { reduce = true; temp = item; }
                }
            }
            return (false,null,null);
        }
        public Lr1Table()
        {
            _table = new Dictionary<(int, Symbol), Actions>(new Comp());
        }

        public Action<dynamic[]> Logger { get; set; }

        public bool TryParse(Grammar g, IEnumerable<Token> tokens, out DerivationTree tree)
        {
            var productions = Parser(g, tokens.ToList());

            if (!Errors.HasError())
            {
                tree = DerivationTree.FromRightMost(productions.Item2);
                return true;
            }
            tree = null;
            return false;
        }

        public void Add((int, Symbol) key, Actions value)
        {
            _table.Add(key, value);
        }

        public void MakeTable(List<Lr1State> a)
        {
            //_table = new Dictionary<(int, Symbol), Actions>();
            //FillTableDefualt(a);
            foreach (var states in a)
            foreach (var item in states.Items)
            {
                if (item.ProductionNumber == 0 && item.DotNumber == 1)
                {
                    (int,EOF) tuple = (states.StateNumber, _automaton.G.EOF);
                    var actions = new Actions
                    {
                        ActionType = ActionType.Accept,
                        ActionParameter = item.ProductionNumber
                    };
                    if (!_table.ContainsKey(tuple))
                        _table.Add(tuple, actions);
                    continue;
                }
                if (item.DotNumber == _automaton.Productions[item.ProductionNumber].Right.Length &&
                    _automaton.Productions[item.ProductionNumber].Left.Name != "S'")
                {
                    foreach (var l in item.Lookahead)
                    {
                        var tuple = (states.StateNumber, l);
                        var actions = new Actions
                        {
                            ActionType = ActionType.Reduce,
                            ActionParameter = item.ProductionNumber
                        };
                        if (!_table.ContainsKey(tuple))
                            _table.Add(tuple, actions);
                    }

                    continue;
                }

                if (_automaton.Productions[item.ProductionNumber].Right[item.DotNumber] is Terminal)
                {
                    var contain = IsGoto(_automaton.Gotos, states.StateNumber,
                        _automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                    if (contain < 0) continue;
                    var t =
                        (states.StateNumber, _automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                    var actions = new Actions
                    {
                        ActionType = ActionType.Shift,
                        ActionParameter = contain
                    };
                    if (!_table.ContainsKey(t))
                        _table.Add(t, actions);
                }

                else
                {
                    var contain = IsGoto(_automaton.Gotos, states.StateNumber,
                        _automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                    if (contain < 0) continue;
                    var t =
                        (states.StateNumber, _automaton.Productions[item.ProductionNumber].Right[item.DotNumber]);
                    var actions = new Actions
                    {
                        ActionType = ActionType.Goto,
                        ActionParameter = contain
                    };
                    if (!_table.ContainsKey(t))
                        _table.Add(t, actions);
                }
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
            foreach (var t in _table)
                if (t.Value.ActionType != ActionType.None)
                    Console.WriteLine("[" + t.Key.Item1 + "," + t.Key.Item2 + "] = " + t.Value.ActionType + " " +
                                      t.Value.ActionParameter);
            return "";
        }

        //public void ParsingError(Token token)
        //{
        //    _errors.Add("Parsing Error in Line: " + token.Line + " Column: " + token.Column + " : " + token.Text + " not match with grammar rules");
        //}

        public (bool okparsed, IEnumerable<(ProductionAttr, List<Token>)>) Parser(Grammar g, List<Token> tokens)
        {
            var ok = true;
            var Prod = new ProductionAttr[g.Productions.Count() + 1];
            var numberproduction = 1;

            foreach (var p in g.Productions)
                Prod[numberproduction++] = p;

            var s1 = g.NonTerminal("S'");
            s1 %= g.StartSymbol;

            Prod[0] = g.Productions.Last();

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
                        var token = tokens[pos++];
                        Logger += p => Console.WriteLine("Parsing Error in Line: " + token.Line + " Column: " +
                                                         token.Column + " : " + token.Text +
                                                         " not match with grammar rules");
                        ok = false;
                        //ParsingError(tokens[pos++]);
                        continue;
                    }
                }
                var t = (s.Item1, temp);
                try { 
                switch (_table[t].ActionType)
                    {
                        case ActionType.Shift:
                            stack.Push((_table[t].ActionParameter, tok));
                            pos++;
                            break;
                        case ActionType.Reduce:
                            var len = Prod[_table[t].ActionParameter].IsEpsilon
                                ? 0
                                : Prod[_table[t].ActionParameter].Right.Length;

                            var toks = new List<Token>();
                            while (len > 0)
                            {
                                var tmp = stack.Pop();
                                if (tmp.Item2 != null)
                                    toks.Add(tmp.Item2);
                                len--;
                            }
                            var top = stack.Peek().Item1;
                            var t2 = (top, Prod[_table[t].ActionParameter].Left as Symbol);

                            if (_table[t2].ActionType == ActionType.Goto)
                                stack.Push((_table[t2].ActionParameter, null));

                            
                            toks.Reverse();
                            result.Add((Prod[_table[t].ActionParameter], new List<Token>(toks)));
                            toks.Clear();
                            
                            break;
                        default:
                            if (_table[t].ActionType == ActionType.Accept)
                                if (ok)
                                {
                                    Console.WriteLine("Accept");
                                    return (true, result);
                                }
                            var token = tokens[pos];
                            Logger += p => Console.WriteLine("Parsing Error in Line: " + token.Line + " Column: " +
                                                             token.Column + " : " + token.Text +
                                                             " not match with grammar rules");
                            return (false, result);
                    }
                }
                catch (Exception)
                {
                    Errors.Log(new ErrorLogger.Error(tok.Line,tok.Column,tok.Text,"Parsing Error"));
                    return (false,result);
                }
            }
        }
    }
}