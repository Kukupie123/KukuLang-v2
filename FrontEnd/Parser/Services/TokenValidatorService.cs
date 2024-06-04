using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public static class TokenValidatorService
{
    public static void validateToken(TokenType tokenType, Token token)
    {
        if (tokenType != token.Type) throw new UnexpectedTokenException(tokenType, token);
    }
    public static void validateToken(TokenType tokenType, dynamic tokenVal, Token token)
    {
        if (tokenType != token.Type && tokenVal != token.Value) throw new UnexpectedTokenException(tokenType, tokenVal, token);
    }

    internal static void validateToken(List<TokenType> tokenTypes, Token token)
    {
        if (tokenTypes.Contains(token.Type) == false)
        {
            List<Token> expectedTokens = [];
            tokenTypes.ForEach(t => expectedTokens.Add(new Token(t, t.ToString(), -1)));
            throw new UnexpectedTokenException(expectedTokens, token);
        }
    }
}
