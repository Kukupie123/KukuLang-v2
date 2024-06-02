namespace FrontEnd;

/// Represents an object such as integer, float, string, custom type.
public class ObjectExp(string type, Dictionary<string, dynamic> objectVariables) : ExpressionStmt(type)
{
    string Type = type;
    /// Integer, Float, String types will store it's value in "Value" key.
    Dictionary<string, dynamic> ObjectVariables = objectVariables;

    public override string ToString()
    {
        return $"{base.ToString()}\n" +
               $"Type: {Type}\n" +
               $"Object Variables:\n{FormatObjectVariables()}";
    }

    private string FormatObjectVariables()
    {
        return string.Join("\n", ObjectVariables.Select(kv => $"  {kv.Key}: {kv.Value}"));
    }
}
