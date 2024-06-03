namespace FrontEnd;

public class ASTScope
{
    public Dictionary<string, CustomType> CustomTypes = new(); //TypeName, CustomType
    public Dictionary<string, CustomTask> CustomTasks = new(); //TaskName, CustomTask
    public List<Stmt> Statements = new();

    // ToString function to provide a string representation of the ASTScope instance
    public override string ToString()
    {
        var customTypesStr = string.Join(", ", CustomTypes.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        var customTasksStr = string.Join(", ", CustomTasks.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
        var statementsStr = string.Join("\n", Statements.Select(stmt => stmt.ToString()));

        return $"ASTScope:\n" +
               $"CustomTypes: {{ {customTypesStr} }}\n" +
               $"CustomTasks: {{ {customTasksStr} }}\n" +
               $"Statements: [\n{statementsStr}\n]";
    }
}