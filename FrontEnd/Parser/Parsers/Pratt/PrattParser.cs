using System.Diagnostics;
using System.IO.Compression;
using FrontEnd.Commons.Tokens;

namespace FrontEnd;

public class PrattParser(List<Token> tokens, int startingPosition = 0) : ParserBase<ExpressionStmt, int>(tokens, startingPosition)
{
    private static readonly Dictionary<string, int> Precedence = new Dictionary<string, int>
    {
        {"Lowest", 1},
        {"Comparison", 2},
        {"Sum", 3},
        {"Product", 4},
        {"BooleanOr", 5},
        {"BooleanAnd", 6}
    };
    private int GetPrecedence(TokenType tokenType)
    {
        switch (tokenType)
        {
            case TokenType.Add:
            case TokenType.Minus:
                return Precedence["Sum"];
            case TokenType.Multiply:
            case TokenType.Divide:
                return Precedence["Product"];
            case TokenType.And:
                return Precedence["BooleanAnd"];
            case TokenType.Or:
                return Precedence["BooleanOr"];
            case TokenType.Comparator:
                return Precedence["Comparison"];
            default:
                return Precedence["Lowest"];
        }
    }
    public override ExpressionStmt Parse(int precedence = 0)
    {
        var left = ProcessPrimaryAndAdvance();
        /*
        After processing primary token check if the next token has higher precedence
        If yes then this "left" expressionStmt will be passed in ProcessInfixAndAdvance function
        That function will wrap it up within it's own expression statement and return the newly created expression statement
        which will have this "left" expression statement wrapped in it. It also usually will recursively call Parse function
        For right expression such as for binary expressions
        */
        if (precedence < GetPrecedence(CurrentToken.Type))
        {
            //Intentionally doing it in two lines to make it easy for viewers to understand.
            var newLeft = ProcessInfixAndAdvance(left);
            left = newLeft;
        }
        return left;
    }



    /*
    Example :- 
    a + b (a and b are primary Token)
    */
    ExpressionStmt ProcessPrimaryAndAdvance()
    {
        var token = CurrentToken;
        Advance();
        if (token.Type is TokenType.Float or TokenType.Integer or TokenType.Text)
        {
            return new ObjectExp(token.Type.ToString(),
            new()
                {
                    { "Value", token.Value}
                }
            );
        }
        throw new Exception($"Can't Process primary token {token}");
    }

    /*
    Example :-
    a + b (+ is an infix since its between two operands)
    */
    private ExpressionStmt ProcessInfixAndAdvance(ExpressionStmt leftExpression)
    {
        var token = CurrentToken;
        Advance();
        switch (token.Type)
        {
            case TokenType.Add:
            case TokenType.Minus:
            case TokenType.Multiply:
            case TokenType.Divide:
            case TokenType.Mod:
                return new BinaryExp(leftExpression, TokenType.Add.ToString(), Parse(GetPrecedence(token.Type)));
        }
        throw new Exception($"Can't process Infix token {token}");
    }
}
