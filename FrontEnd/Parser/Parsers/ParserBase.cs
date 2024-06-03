using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public abstract class ParserBase<ParserReturnType, ParserArgument>(List<Token> tokens, int startingPosition = 0)
{
    protected int _Pos = startingPosition;
    protected readonly List<Token> _Tokens = tokens;

    public Token CurrentToken => _Tokens[_Pos];

    public Token ConsumeCurrentToken()
    {
        Token current = CurrentToken;
        _Pos++;
        if (_Pos >= _Tokens.Count)
        {
            throw new Exception("Unexpected end of input");
        }
        return current;
    }

    public void Advance() => _Pos++;

    public abstract ParserReturnType Parse(ParserArgument arg);
}
