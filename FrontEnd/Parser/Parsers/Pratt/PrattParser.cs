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
    public override ExpressionStmt Parse(int precedence = 1)
    {
        var left = ProcessPrimaryExpAndAdvance();

        /*
        Eg :- Set Kuku to 12.
        Kuku is wrapped as VariableExpression and the next token is "to" so we need to exit
        */
        if (left is VariableExp && CurrentToken.Type == TokenType.To)
            return left;

        /*
        In the Parse method, we initially parse the left-hand side of the expression using ProcessPrimaryExpAndAdvance.
        We then check if the current token's precedence is higher than the specified precedence level.

            Explanation with an example:
                Suppose we have the expression "5 * 5 / 5" with the default precedence level being 1.
                When we are parsing the '*' operator with a precedence level of 3 in the Parse method,
                the next infix operator is '/', which also has a precedence level of 3.
                However, the condition (precedence < GetPrecedence(CurrentToken.Type)) is never true since they are the same (3 == 3).
                If we used an "if" statement here, it would ignore parsing the right-hand side of the expression ("/ 5").
                This is because the "if" statement, after obtaining the newLeft (result of '5 * 5'), would simply exit and return,
                not considering the next token ('/') or further parsing.

                Using a "while" loop instead ensures that we continue parsing the entire expression correctly.
                The loop keeps iterating until we reach a token with lower or equal precedence than the specified level,
                allowing us to handle all parts of the expression with the correct precedence order.

            In summary, using a "while" loop is crucial to ensure that we properly parse the entire expression
            and handle operators with different precedence levels in the correct order.
        */
        while (precedence < GetPrecedence(CurrentToken.Type))
        {
            //If we hit a full stop that is the end of the expression
            if (CurrentToken.Type == TokenType.FullStop)
            {
                return left;
            }

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
        var token = ConsumeCurrentToken();

        if (token.Type is TokenType.FloatLiteral or TokenType.IntegerLiteral or TokenType.TextLiteral)
        {
            return new ObjectExp(token.Type.ToString(),
            new()
                {
                    { "Value", token.Value }
                }
            );
        }
        if (token.Type == TokenType.Accessor)
        {
            /*
            If we are at 's then the next token has to be an identifier
            Eg :- Set kuku's name to "kuku".
            We are at 's and the next identifier is "name"
            */
            if (CurrentToken.Type != TokenType.Identifier)
                throw new Exception($"Expected identifier token after 's but got {CurrentToken}");
            return ProcessPrimaryExpAndAdvance();
        }
        if (token.Type == TokenType.Identifier)
        {
            //Eg:-Set a to 12.
            //Eg:-Set Kuku's name to "kuku".
            /*
            Token = a/kuku
            CurrentToken = to/'s
            */

            if (CurrentToken.Type == TokenType.Accessor)
            {
                return new VariableExp(token.Value, ProcessPrimaryExpAndAdvance() as VariableExp);

            }
            if (CurrentToken.Type is TokenType.To or TokenType.FullStop)
            {
                return new VariableExp(token.Value, null);
            }
            throw new Exception($"Expected either an 's or to token but got {CurrentToken}");
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
            ConsumeCurrentToken(); // Consume the closing bracket that matches this opening bracket.
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
        var token = ConsumeCurrentToken();
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
