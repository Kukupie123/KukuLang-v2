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
        { "set", TokenType.Keyword },
        { "if", TokenType.Keyword },
        { "then", TokenType.Keyword },
        { "else", TokenType.Keyword },
        { "repeat", TokenType.Keyword },
        { "until", TokenType.Keyword },
        { "as_long_as", TokenType.Keyword },
        { "define", TokenType.Keyword },
        { "function", TokenType.Keyword },
        { "called", TokenType.Keyword },
        { "with", TokenType.Keyword },
        { "parameters", TokenType.Keyword },
        { "as", TokenType.Keyword },

        { "nothing", TokenType.Null },

        { "to", TokenType.Keyword },

        { "is", TokenType.Comparator },
        { "is_not", TokenType.Comparator },
        { "is_greater_than", TokenType.Comparator },
        { "is_greater_or_equal_to", TokenType.Comparator },
        { "is_less_than", TokenType.Comparator },
        { "is_less_or_equal_to", TokenType.Comparator },

        { "and", TokenType.Logical },
        { "or", TokenType.Logical },

        { "{", TokenType.Parenthesis },
        { "[", TokenType.Parenthesis },
        { "(", TokenType.Parenthesis },
        { "}", TokenType.Parenthesis },
        { "]", TokenType.Parenthesis },
        { ")", TokenType.Parenthesis },

        { " ", TokenType.Terminator },
        { ",", TokenType.Terminator },
        { ";", TokenType.Terminator },
        { "\n", TokenType.Terminator }, //newline
        { "\r", TokenType.Terminator }, //newline

        { "-", TokenType.Math },
        { "+", TokenType.Math },
        { "/", TokenType.Math },
        { "*", TokenType.Math },
        { "%", TokenType.Math },
    };
}