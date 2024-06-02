namespace FrontEnd;

public class BinaryExp(ExpressionStmt left, string op, ExpressionStmt right) : ExpressionStmt("BinaryExp")
{
    ExpressionStmt Left = left;
    string Op = op;
    ExpressionStmt Right = right;
}
