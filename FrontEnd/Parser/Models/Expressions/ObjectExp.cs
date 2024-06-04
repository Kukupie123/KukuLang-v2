namespace FrontEnd
{
    /// <summary>
    /// Represents an object such as integer, float, string, custom type.
    /// </summary>
    public class ObjectExp : ExpressionStmt
    {
        public string Type { get; }
        public Dictionary<string, dynamic> ObjectVariables { get; }

        public ObjectExp(string type, Dictionary<string, dynamic> objectVariables)
            : base(type)
        {
            Type = type;
            ObjectVariables = objectVariables;
        }

        public override string ToString(int indentLevel = 0)
        {
            var formattedObjectVariables = string.Join(", ", ObjectVariables.Select(kv => $"{IndentHelper.Indent($"{kv.Key}: {kv.Value}", indentLevel + 2)}"));
            var indent = IndentHelper.Indent("", indentLevel);
            return $"{indent}{Type}->Object Variables: {{ {formattedObjectVariables} }}";
        }
    }
}