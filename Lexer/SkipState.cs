using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    class SkipState : State
    {
        public SkipState(int tokenPriority) : base(tokenPriority)
        {

        }
        public State next;
        public override void AddLink(char q, State s)
        {
            next = s;
        }
        public override IEnumerable<State> GetLinks(char q)
        {

            return q != 'ε' ? new List<State> { next } : new List<State>();
        }
    }
}
