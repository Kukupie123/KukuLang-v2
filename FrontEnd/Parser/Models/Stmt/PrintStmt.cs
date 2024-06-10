using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Stmt;

namespace KukuLang.Parser.Models.Stmt
{
    public class PrintStmt : StmtBase
    {
        public ExpressionStmt Expression { get; }

        public PrintStmt(ExpressionStmt expression) : base("Print Statement")
        {
            Expression = expression;
        }

        public override string ToString(int indentLevel = 0)
        {
            string indent = new string(' ', indentLevel);
            string expressionStr = Expression.ToString(indentLevel + 2);

            return $"{indent}Print:\n{expressionStr}";
        }
    }
}
