namespace FrontEnd;

public class BinaryExp(ExpressionStmt left, string op, ExpressionStmt right) : ExpressionStmt("BinaryExp")
{
    ExpressionStmt Left = left;
    string Op = op;
    ExpressionStmt Right = right;

    public override string ToString()
    {
        return $"{base.ToString()}\n" +
               $"Operator: {Op}\n" +
               $"Left:\n{Indent(Left.ToString(), 2)}\n" +
               $"Right:\n{Indent(Right.ToString(), 2)}";
    }

    private string Indent(string text, int spaces)
    {
        var padding = new string(' ', spaces);
        return padding + text.Replace("\n", "\n" + padding);
    }
}
