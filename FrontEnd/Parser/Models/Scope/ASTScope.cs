namespace FrontEnd
{
    public class ASTScope
    {
        public Dictionary<string, CustomType> CustomTypes { get; } = new();
        public Dictionary<string, CustomTask> CustomTasks { get; } = new();
        public List<Stmt> Statements { get; } = new();

        public string ToString(int indentLevel = 0)
        {
            var customTypesStr = string.Join("\n", CustomTypes.Select(kvp => kvp.Value.ToString(indentLevel + 2)));
            var customTasksStr = string.Join("\n", CustomTasks.Select(kvp => kvp.Value.ToString(indentLevel + 2)));
            var statementsStr = string.Join("\n", Statements.Select(stmt => stmt.ToString(indentLevel + 2)));

            return $"ASTScope:\n{customTypesStr}\n{customTasksStr}\n{statementsStr}";
        }
    }
}