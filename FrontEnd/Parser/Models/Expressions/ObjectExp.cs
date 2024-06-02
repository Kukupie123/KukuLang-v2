namespace FrontEnd;

///Represents an object such as integer, float, string, custom type.
public class ObjectExp(string type, Dictionary<string, dynamic> objectVariables) : ExpressionStmt(type)
{
    string Type = type;
    Dictionary<string, dynamic> ObjectVariables = objectVariables;
}
