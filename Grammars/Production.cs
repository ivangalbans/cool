namespace Grammars
{
    public class Production
    {
        public Production(NonTerminal nonTerminal, Sentence sentence)
        {
            Left = nonTerminal;
            Right = sentence;
        }

        public NonTerminal Left { get; }
        public Sentence Right { get; }

        public bool IsEpsilon => Right.Length == 1 && Right[0] == Left.Grammar.Epsilon;


        public override string ToString()
        {
            return $"{Left} --> {Right}";
        }
    }
}