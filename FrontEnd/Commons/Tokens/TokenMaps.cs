namespace FrontEnd.Commons.Tokens;

public class TokenMap
{
    /// <summary>
    /// Returns all the words that are associated with the type
    /// </summary>
    /// <returns></returns>
    public static List<string> GetValuesByType(TokenType type)
    {
        List<string> values = [];

        foreach (var (word, tokenType) in Map)
        {
            if (tokenType == type) values.Add(word);
        }

        return values;
    }

    public static TokenType GetTypeByValue(string value)
    {
        return Map[value];
    }

    public static bool IsValueAGenericToken(string value)
    {
        return Map.ContainsKey(value);
    }


    private static readonly Dictionary<string, TokenType> Map = new()
    {
        { "Define", TokenType.Define },
        { "with", TokenType.With },
        { "returning", TokenType.Returning },
        { "as", TokenType.As },
        { "'", TokenType.AccessorPRIVATE }, // For use in lexer.
        { "'s", TokenType.Accessor },
        { "variable", TokenType.Keyword },
        { "function", TokenType.Keyword },
        { "of_type", TokenType.Keyword },
        { "new", TokenType.Keyword },
        { "Set", TokenType.Set },
        { "to", TokenType.Keyword },
        { "if", TokenType.Conditional },
        { "then", TokenType.Keyword },
        { "else", TokenType.Keyword },
        { "repeat", TokenType.Keyword },
        { "until", TokenType.UntilLoop },
        { "as_long_as", TokenType.AsLongAsLoop },

        { "is", TokenType.Comparator },
        { "is_not", TokenType.Comparator },
        { "is_greater_than", TokenType.Comparator },
        { "is_greater_or_equal_to", TokenType.Comparator },
        { "is_less_than", TokenType.Comparator },
        { "is_less_or_equal_to", TokenType.Comparator },

        { "and", TokenType.Logical },
        { "or", TokenType.Logical },

        { "{", TokenType.CurlyBracesOpening },
        { "[", TokenType.SquareBracketsOpening },
        { "(", TokenType.RoundBracketsOpening },
        { "}", TokenType.CurlyBracesClosing },
        { "]", TokenType.SquareBracketsClosing},
        { ")", TokenType.RoundBracketsClosing },

        { ",", TokenType.Comma },
        { ".", TokenType.Terminator },

        { "nothing", TokenType.Nothing },

        { "-", TokenType.Math },
        { "+", TokenType.Math },
        { "/", TokenType.Math },
        { "*", TokenType.Math },
        { "%", TokenType.Math },
    };
}