using System.IO.Compression;
using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public class PrattParser(List<Token> tokens, int startingPosition = 0) : ParserBase(tokens, startingPosition)
{
    public override Scope Parse()
    {
        throw new NotImplementedException();
    }
}
