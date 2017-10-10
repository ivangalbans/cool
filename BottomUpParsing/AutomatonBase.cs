using System.Collections.Generic;
using Grammars;

namespace BottomUpParsing
{
    public interface IState<out T>
    {
        T state { get; }
    }
    public abstract class AutomatonBase
    {
        public Grammar G { get; }
        internal List<LrState> Automaton { get; }
        internal ProductionAttr[] Productions { get; }
        private Queue<LrState> LrStatesQueue { get; }
        private int CurrentState { get; }
        public abstract void MakeAutomaton();
    }
}