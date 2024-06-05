using FrontEnd.Parser.Services;

namespace FrontEnd.Parser.Models.Expressions
{
    /// <summary>
    /// Represents an object such as integer, float, string, custom type.
    /// </summary>
    public class ObjectExp(string type, Dictionary<string, dynamic> objectVariables) : ExpressionStmt(type)
    {
        public string ObjType { get; } = type;
        public Dictionary<string, dynamic> ObjectVariables { get; } = objectVariables;

        public override string ToString(int indentLevel = 0)
        {
            string type = IndentHelper.Indent(ObjType + "\n", 0); // Why does indent being 0 work wtf?
            string variablesStr = IndentHelper.Indent("Variables : \n", indentLevel + 2);

            foreach (var k in ObjectVariables)
            {
                variablesStr += IndentHelper.Indent($"{k.Key} : {k.Value}", indentLevel + 4);
            }

            // Improve variable handling:
            variablesStr = IndentHelper.Indent("Properties:\n", indentLevel) +
                           string.Join("\n", ObjectVariables.Select(p => IndentHelper.Indent(p.ToString(), indentLevel + 2)));


            return IndentHelper.Indent(type + variablesStr, indentLevel);
        }

    }
}