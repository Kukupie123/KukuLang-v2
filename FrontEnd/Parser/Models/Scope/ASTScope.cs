namespace FrontEnd
{
    public class ASTScope
    {
        public Dictionary<string, CustomType> CustomTypes { get; } = new(); // TypeName, CustomType
        public Dictionary<string, CustomTask> CustomTasks { get; } = new(); // TaskName, CustomTask
        public List<Stmt> Statements { get; } = new();

        // ToString function to provide a string representation of the ASTScope instance
        public override string ToString()
        {
            var customTypesStr = string.Join("\n", CustomTypes.Select(kvp => $"  {kvp.Key}: {kvp.Value}"));
            var customTasksStr = string.Join("\n", CustomTasks.Select(kvp => $"  {kvp.Key}: {kvp.Value}"));
            var statementsStr = string.Join("\n", Statements.Select(stmt => $"  {{\n{stmt.ToString()}\n  }}"));

            return $"ASTScope:\n" +
                   $"CustomTypes:\n{customTypesStr}\n" +
                   $"CustomTasks:\n{customTasksStr}\n" +
                   $"Statements:\n{statementsStr}\n";
        }
    }
}
