namespace Grammars
{
    public class Token
    {
        public Token(int id, string type, string text, int line, int column)
        {
            Id = id;
            Type = type;
            Text = text;
            Line = line;
            Column = column;
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public override string ToString()
        {
            return $"Type: {Type}, Text: {Text}, Line: {Line}, Column: {Column}";
        }
    }
}