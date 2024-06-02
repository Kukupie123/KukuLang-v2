using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public class RecursiveDescentParser(List<Token> tokens, int startingPosition = 0) : ParserBase<Scope, dynamic>(tokens, startingPosition)
{
    public override Scope Parse(dynamic arg)
    {
        return null;
    }
}
