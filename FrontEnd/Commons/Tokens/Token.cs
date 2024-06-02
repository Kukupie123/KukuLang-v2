namespace FrontEnd.Commons.Tokens;

public class Token
{
    public TokenType Type;
    public dynamic Value;

    public int Position;

    public Token(TokenType type, dynamic value, int position)
    {
        Type = type;
        Value = value;
        Position = position;
    }

    public override string ToString()
    {
        return $"Token:{Position}:{Type} : {Value}";
    }
}