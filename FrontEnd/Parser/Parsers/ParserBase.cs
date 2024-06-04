using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public abstract class ParserBase<ParserReturnType, ParserArgument>(List<Token> tokens, int startingPosition = 0)
{
    public int _Pos = startingPosition;
    public readonly List<Token> Tokens = tokens;

    public Token CurrentToken => Tokens[_Pos];

    public Token ConsumeCurrentToken()
    {
        Token current = CurrentToken;
        _Pos++;
        if (_Pos >= Tokens.Count)
        {
            throw new Exception("Unexpected end of input");
        }
        return current;
    }

    public void Advance() => _Pos++;

    public abstract ParserReturnType Parse(ParserArgument arg);
}
