using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public abstract class ParserBase<T, F>(List<Token> tokens, int startingPosition = 0)
{
    protected int _Pos = startingPosition;
    protected readonly List<Token> _Tokens = tokens;

    protected Token CurrentToken => _Tokens[_Pos];
    protected void Advance() => _Pos++;

    public abstract T Parse(F arg);
}
