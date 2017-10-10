using System;

namespace Error
{
    public interface IError
    {
        Action<dynamic[]> Logger { get; set; }
        //  void ParsingError(Token token);
    }
}