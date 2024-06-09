﻿using FrontEnd.Parser.Models.Expressions;
using FrontEnd.Parser.Models.Scope;
using FrontEnd.Parser.Models.Stmt;

namespace KukuLang.Parser.Models.Stmt
{
    public class LoopStmt(ExpressionStmt condition, bool isUntil, ASTScope scope) : StmtBase("Loop StmtBase")
    {
        public ExpressionStmt Condition = condition;
        public ASTScope Scope = scope;
        public bool IsUntil = isUntil;

        public override string ToString(int indentLevel = 0)
        {
            string indent = new string(' ', indentLevel);
            string conditionIndent = new string(' ', indentLevel + 2);
            string scopeIndent = new string(' ', indentLevel + 4);

            string conditionStr = Condition.ToString(indentLevel + 2);
            string scopeStr = Scope.ToString(indentLevel + 4);

            return $"{indent}{(IsUntil ? "Until" : "As_LongAs")}\n{conditionIndent}{conditionStr}\n{conditionIndent}then\n{scopeIndent}{scopeStr}";
        }
    }
}