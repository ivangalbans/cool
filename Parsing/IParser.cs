using System;
using System.Collections.Generic;
using Grammars;

namespace Parsing
{
    public interface IParser
    {
        bool TryParse(Grammar g, IEnumerable<Token> tokens, out DerivationTree tree);
    }

    public class ParsingException : Exception
    {
    }
}