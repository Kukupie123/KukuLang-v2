using FrontEnd.Commons.Tokens;
using System.Text;

namespace FrontEnd.Parser.Models.Exceptions;

public class UnexpectedTokenException : Exception
{
    readonly string Msg = "";
    public UnexpectedTokenException(TokenType expectedTokenType, Token actualToken)
    {
        Msg = $"Expected Token of type: {expectedTokenType} but got token: {actualToken}";
    }
    public UnexpectedTokenException(TokenType expectedTokenType, dynamic expectedTokenVal, Token actualToken)
    {
        Msg = $"Expected Token of type: {new Token(expectedTokenType, expectedTokenVal, -1, -1)} but got token: {actualToken}";
    }
    public UnexpectedTokenException(Token expectedToken, Token actualToken)
    {
        Msg = $"Expected Token: {expectedToken} but got token: {actualToken}";
    }

    public UnexpectedTokenException(List<Token> expectedTokens, Token actualToken)
    {
        var sb = new StringBuilder();
        foreach (var t in expectedTokens)
        {
            sb.Append(t + " | ");
        }
        Msg = $"Expected Tokens: {sb} but got token: {actualToken}";
    }
    public override string Message => Msg;
}
