using FrontEnd.Parser.Models.Stmt;
using KukuLang.Interpreter.Model.Scope;

namespace KukuLang.Interpreter.Handler
{
    public static class Handler
    {
        public static void StatementHandler(Stmt statement, RuntimeScope scope)
        {
            switch (statement)
            {
                case SetToStmt stmt:
                    SetToStmtHandler(stmt, scope);
                    break;
                case IfStmt:
                    break;
                case FunctionCallStmt:
                    break;
            }
        }

        private static void SetToStmtHandler(SetToStmt stmt, RuntimeScope scope)
        {
            /*
             * ToSetVar can be a new variable with no next node
             * Or it can be setting of exist var if it has node
             */

            /*
             * VarVal can be only variable or new instantiation of Custom Type
             * We can determine this if it has no next node, if it is not found in objects list it is a Custom type instantiation or else it's a var reference
             * If it has next node we need to get the obj's value each level
             */

            /*
             * It's best if this process is handled separately in StmtHandler class
             */
        }

    }
}
