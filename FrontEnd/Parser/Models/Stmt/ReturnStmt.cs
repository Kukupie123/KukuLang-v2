using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Stmt;


public class ReturnStmt(ExpressionStmt? expression) : StmtBase("Return StmtBase")
{
    public ExpressionStmt? Expression = expression;
}

