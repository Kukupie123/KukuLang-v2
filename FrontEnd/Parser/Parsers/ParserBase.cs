using FrontEnd.Commons.Tokens;
using System.Diagnostics;

namespace FrontEnd.Parser.Parsers;

public abstract class ParserBase<ParserReturnType, ParserArgument>(List<Token> tokens, int startingPosition = 0)
{
    public int _Pos = startingPosition;
    public readonly List<Token> Tokens = tokens;

    public Token CurrentToken => Tokens[_Pos];

    public Token ConsumeCurrentToken(int offset = 1)
    {
        Token current = CurrentToken;
        Advance(offset);
        if (_Pos >= Tokens.Count)
        {
            throw new Exception("Unexpected end of input");
        }
        return current;
    }

    public void Advance(int offset = 1) => _Pos += offset;

    public Token Peek(int offset = 1)
    {

        var token = Tokens[_Pos + offset];
        Debug.WriteLine($"Peeking {token}");
        return token;
    }

    public abstract ParserReturnType Parse(ParserArgument arg);
}
