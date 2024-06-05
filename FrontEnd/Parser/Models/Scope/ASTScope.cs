

using FrontEnd.Parser.Services;

namespace FrontEnd.Parser.Models.Scope
{
    public class ASTScope(string scopeName)
    {
        public string ScopeName = scopeName;
        public Dictionary<string, CustomType.CustomType> CustomTypes { get; } = [];
        public Dictionary<string, CustomTask.CustomTask> CustomTasks { get; } = [];
        public List<Stmt.Stmt> Statements { get; } = [];

        public string ToString(int indentLevel = 0)
        {
            string scopeNameStr = IndentHelper.Indent(ScopeName + ":", indentLevel) + "\n";
            string typesStr = $"{IndentHelper.Indent("Types:", indentLevel + 2)}\n";
            foreach (var t in CustomTypes)
            {
                typesStr += t.Value.ToString(indentLevel + 4);
            }
            typesStr += "\n";
            string tasksStr = $"{IndentHelper.Indent("Tasks:", indentLevel + 2)}\n";
            foreach (var t in CustomTasks)
            {
                tasksStr += t.Value.ToString(indentLevel + 4);
            }
            tasksStr += "\n";
            string stmtStr = $"{IndentHelper.Indent("Statements:", indentLevel + 2)}\n";
            foreach (var t in Statements)
            {
                stmtStr += t.ToString(indentLevel + 4) + "\n\n";
            }
            string final = scopeNameStr + typesStr + tasksStr + stmtStr;
            return final + "\n";
        }
    }
}