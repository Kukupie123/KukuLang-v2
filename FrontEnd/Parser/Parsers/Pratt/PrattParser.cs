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

    /*
    The Pratt Parser is a top-down operator precedence parser that processes expressions 
    based on the precedence of operators. It works by:

    1. Parsing the left-most primary expression (e.g., a number or identifier).
    2. Checking the precedence of the next token.
    3. If the next token has a higher precedence, it recursively parses the right-hand 
       side of the expression, combining it with the left-most expression using the operator.
    4. This process continues, with the parser always combining the most recently parsed 
       expression with the next token based on its precedence.

    For example, in the expression "a + b * c":
    - It first parses "a" (the left-most primary expression).
    - It then sees the "+" operator and checks its precedence.
    - Since "*" has higher precedence than "+", it recursively parses "b * c".
    - It then combines "a" with the result of "b * c" using the "+" operator.
    */
    public override ExpressionStmt Parse(int precedence = 0)
    {
        var left = ProcessPrimaryExpAndAdvance();

        //If we hit a full stop that is the end of the expression
        if (CurrentToken.Type == TokenType.FullStop)
        {
            return left;
        }
        if (precedence < GetPrecedence(CurrentToken.Type))
        {
            // Intentionally doing it in two lines to make it easy for viewers to understand.
            var newLeft = ProcessInfixAndAdvance(left);
            left = newLeft;
        }
        return left;
    }

    /*
    Example:
    a + b (a and b are primary tokens)
    */
    ExpressionStmt ProcessPrimaryExpAndAdvance()
    {
        var token = CurrentToken;
        Advance();

        if (token.Type is TokenType.Float or TokenType.Integer or TokenType.Text)
        {
            return new ObjectExp(token.Type.ToString(),
            new()
                {
                    { "Value", token.Value }
                }
            );
        }
        if (token.Type is TokenType.RoundBracketsOpening)
        {
            /*
            When encountering an opening bracket, we need to parse the entire sub-expression within the brackets.
            This call to Parse(GetPrecedence(token.Type)) processes the expression inside the brackets with the current precedence level.

            For example, in the expression (12 + 1) * 5:
            - The parser encounters the opening bracket '(' and calls Parse to handle the sub-expression "12 + 1".
            - The Parse method will continue processing until it reaches the closing bracket ')'.

            The reason the parsing stops correctly at the closing bracket ')' is due to how precedence is handled:
            - The condition for infix processing in the main Parse method checks if the current precedence is less than the next token's precedence.
            - The precedence of the current token inside the brackets (e.g., '1' in "12 + 1") is compared to the precedence of the closing bracket ')'.
            - Since both have the lowest precedence (or 0 in this implementation), the parser correctly returns the parsed sub-expression "12 + 1".

            This ensures that the sub-expression within the brackets is fully parsed before continuing with the rest of the expression.
            */
            var blockExpression = Parse(GetPrecedence(token.Type));
            Advance(); // Consume the closing bracket that matches this opening bracket.
            return blockExpression;
        }

        throw new Exception($"Can't process primary token {token}");
    }

    /*
    Example:
    a + b (+ is an infix since it's between two operands)
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
                /*
                Call Parse with the current token's precedence level to parse the right-hand side of the expression.
                This is necessary to correctly handle operators with different precedence levels.

                For example, in the expression "a + b * c":
                - After parsing the left-most primary expression "a", we encounter the "+" operator.
                - The parser needs to determine the right-hand side of this "+" operation.
                - Since "*" has higher precedence than "+", the parser should treat "b * c" as a single unit.
                - Therefore, Parse is called with the precedence of the "*" operator to ensure "b * c" is parsed first.
                - This way, the parser correctly combines "a" with the result of "b * c" using the "+" operator, ensuring the correct order of operations.

                The Parse call here is crucial to maintaining the proper precedence rules within the expression.
                */
                return new BinaryExp(leftExpression, token.Type.ToString(), Parse(GetPrecedence(token.Type)));
        }
        throw new Exception($"Can't process infix token {token}");
    }

}
