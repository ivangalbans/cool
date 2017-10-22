using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grammars;
using Lexer;

namespace Core
{
    class CoolLexer : ILexer<Token>
    {
        public IEnumerable<Token> Lex(string str, Grammar g)
        {
            throw new NotImplementedException();
        }
    }
}
