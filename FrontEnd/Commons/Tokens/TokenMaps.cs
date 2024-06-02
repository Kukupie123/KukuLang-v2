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
        { "Define", TokenType.Define},

        { "with", TokenType.With},

        { ",", TokenType.Comma},

        { ".", TokenType.FullStop},

        { "returning", TokenType.Returning},

        {"nothing", TokenType.Nothing},

        { "Set", TokenType.Set},

        {"to", TokenType.To},

        { "output",TokenType.Output},

        {"'", TokenType.AccessorPRIVATE},
        {"'s", TokenType.Accessor},

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

        { "-", TokenType.Math },
        { "+", TokenType.Math },
        { "/", TokenType.Math },
        { "*", TokenType.Math },
        { "%", TokenType.Math },
    };
}