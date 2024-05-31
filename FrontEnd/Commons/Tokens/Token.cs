namespace FrontEnd.Commons.Tokens;

public class Token
{
    public TokenType Type;
    public dynamic Value;

    public Token(TokenType type, dynamic value)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"Token({Type} : {Value})";
    }
}