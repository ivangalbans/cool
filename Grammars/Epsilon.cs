namespace Grammars
{
    public class Epsilon : Terminal
    {
        public Epsilon(Grammar g) : base("epsilon", g)
        {
        }

        public override string ToString()
        {
            return "ε";
        }
    }
}