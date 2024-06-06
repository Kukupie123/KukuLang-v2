using FrontEnd.Commons.Tokens;

namespace FrontEnd.Parser.Models.Exceptions
{
    public class NoPropertyException(Token token) : Exception
    {
        private readonly string Msg = $"No Property or Parameter for Definition : {token.Value} at position {token.Position}";
        public override string Message => Msg;
    }
}