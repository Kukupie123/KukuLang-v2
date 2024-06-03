using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public class NoParameterOrPropertyException(Token token) : Exception
{
    private string Msg = $"No Property or Parameter for Definition : {token.Value} at position {token.Position}";
    public override string Message => Msg;
}
