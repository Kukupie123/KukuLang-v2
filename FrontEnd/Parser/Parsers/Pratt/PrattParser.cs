using System.Diagnostics;
using System.IO.Compression;
using FrontEnd.Commons.Tokens;

namespace FrontEnd;

enum Precedence
{
    Lowest = 1,
    Comparison = 2, // Comparators: ==, !=, <, <=, >, >=
    Sum = 3,//+ -
    Product = 4, // / *
    BooleanOr = 5, // ||
    BooleanAnd = 6, // &&
}

public class PrattParser(List<Token> tokens, int startingPosition = 0) : ParserBase<ExpressionStmt, int>(tokens, startingPosition)
{
    public override ExpressionStmt Parse(int precedence)
    {
        ProcessPrimary();
        return null;
    }

    /*
    Example :- 
    a + b (a and b are primary Token)
    */
    ExpressionStmt ProcessPrimary()
    {
        var token = CurrentToken;
        if (token.Type is TokenType.Float or TokenType.Integer or TokenType.Text)
        {
            return new ObjectExp(token.Type.ToString(), token.Value);
        }

        throw new Exception($"Can't Process primary token {token}");
    }
}
