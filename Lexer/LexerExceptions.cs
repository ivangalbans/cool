using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public class LexerExceptions : Exception
    {
        public LexerExceptions(string message) : base(message) { }
    }
}
