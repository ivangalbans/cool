namespace Grammars
{
    public class EOF : Terminal
    {
        public EOF(Grammar g)
            : base("$", g)
        {
        }
    }
}