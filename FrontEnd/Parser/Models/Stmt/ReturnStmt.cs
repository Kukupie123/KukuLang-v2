using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Stmt;


public class ReturnStmt(ExpressionStmt? expression) : Stmt("Return Stmt")
{
    public ExpressionStmt? Expression = expression;
}

