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
        { "define", TokenType.Define},

        { "with", TokenType.With},

        { "if", TokenType.If},

        { "then", TokenType.Then},

        { ",", TokenType.Comma},

        { ".", TokenType.FullStop},

        { "returning", TokenType.Returning},

        {"nothing", TokenType.Nothing},

        {"repeat", TokenType.Repeat},


        { "set", TokenType.Set},

        {"to", TokenType.To},

        { "output",TokenType.Output},

        {"'", TokenType.AccessorPRIVATE},
        {"'s", TokenType.Accessor},

        { "as_long_as", TokenType.AsLongAs },
        { "until", TokenType.Until },


        { "is", TokenType.Comparator },
        { "is_not", TokenType.Comparator },
        { "is_greater_than", TokenType.Comparator },
        { "is_greater_or_is", TokenType.Comparator },
        { "is_less_than", TokenType.Comparator },
        { "is_less_or_is", TokenType.Comparator },

        { "and", TokenType.And },
        { "or", TokenType.Or },

        { "{", TokenType.CurlyBracesOpening },
        { "[", TokenType.SquareBracketsOpening },
        { "(", TokenType.RoundBracketsOpening },
        { "}", TokenType.CurlyBracesClosing },
        { "]", TokenType.SquareBracketsClosing},
        { ")", TokenType.RoundBracketsClosing },

        { "-", TokenType.Minus },
        { "+", TokenType.Add },
        { "/", TokenType.Divide },
        { "*", TokenType.Multiply },
        { "%", TokenType.Mod },
    };
}