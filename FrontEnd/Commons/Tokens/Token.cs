namespace FrontEnd.Commons.Tokens;

public class Token(TokenType type, dynamic value, int position, int linePosition)
{
    public TokenType Type = type;
    public dynamic Value = value;

    public int Position = position;
    public int LinePosition = linePosition;

    public override string ToString()
    {
        return $"Token: {LinePosition} : {Position} : {Type} : {Value}";
    }
}