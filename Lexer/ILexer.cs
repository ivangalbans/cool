using System.Collections.Generic;
using Grammars;

namespace Lexer
{
    public interface ILexer<out T>
    {
        IEnumerable<T> Lex(string str, string g);
    }
}