using System.Text;
using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public class UnexpectedTokenException : Exception
{
    string MSG = "";
    public UnexpectedTokenException(TokenType expectedTokenType, Token actualToken)
    {
        MSG = $"Expected Token of type: {expectedTokenType} but got token: {actualToken}";
    }
    public UnexpectedTokenException(TokenType expectedTokenType, dynamic expectedTokenVal, Token actualToken)
    {
        MSG = $"Expected Token of type: {new Token(expectedTokenType, expectedTokenVal, -1)} but got token: {actualToken}";
    }
    public UnexpectedTokenException(Token expectedToken, Token actualToken)
    {
        MSG = $"Expected Token: {expectedToken} but got token: {actualToken}";
    }

    public UnexpectedTokenException(List<Token> expectedTokens, Token actualToken)
    {
        var sb = new StringBuilder();
        foreach (var t in expectedTokens)
        {
            sb.Append(t + " | ");
        }
        MSG = $"Expected Tokens: {sb} but got token: {actualToken}";
    }
    public override string Message => MSG;
}
