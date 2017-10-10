using Grammars;

namespace BottomUpParsing
{
    public class Goto
    {
        public Goto(int state, Symbol token, int next)
        {
            StateNumber = state;
            NextToken = token;
            NextState = next;
        }

        public int StateNumber { get; set; }
        public Symbol NextToken { get; set; }
        public int NextState { get; set; }
    }
}