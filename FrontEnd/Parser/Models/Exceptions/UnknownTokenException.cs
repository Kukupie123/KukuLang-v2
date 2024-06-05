

using FrontEnd.Commons.Tokens;

namespace FrontEnd.Parser.Models.Exceptions
{
    public class UnknownTokenException(Token token) : Exception($"Unknown token {token}");
      
}
