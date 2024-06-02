using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public abstract class ParserBase(List<Token> tokens, int startingPosition = 0)
{
    private int _Pos = startingPosition;
    private readonly List<Token> _Tokens = tokens;

    private Token CurrentToken => _Tokens[_Pos];
    private void Advance() => _Pos++;

    public abstract Scope Parse();
}
