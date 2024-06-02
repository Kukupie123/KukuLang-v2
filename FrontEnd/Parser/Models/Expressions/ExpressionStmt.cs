namespace FrontEnd;

public abstract class ExpressionStmt(string type)
{
    String Type = type;

    public override string ToString()
    {
        return $"Type : {Type}";
    }
}
