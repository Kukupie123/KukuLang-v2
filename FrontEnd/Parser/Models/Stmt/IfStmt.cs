using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Scope;

namespace FrontEnd.Parser.Models.Stmt
{
    public class IfStmt(ExpressionStmt condition, ASTScope scope) : Stmt("If Statement")
    {
        public ExpressionStmt Condition { get; } = condition;
        public ASTScope Scope { get; } = scope;

        public override string ToString(int indentLevel = 0)
        {
            string indent = new(' ', indentLevel);
            string conditionIndent = new(' ', indentLevel + 2);

            string conditionStr = Condition.ToString(indentLevel + 2);
            string scopeStr = Scope.ToString(indentLevel + 4);

            return $"{indent}if\n{conditionIndent}{conditionStr}\n{conditionIndent}then\n{scopeStr}";
        }
    }
}
