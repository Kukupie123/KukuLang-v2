namespace FrontEnd;

public class Scope
{
    Dictionary<string, CustomType> CustomTypes = new(); //TypeName, CustomType
    Dictionary<string, CustomTask> CustomTasks = new(); //TaskName, CustomTask

    List<Stmt> Statements = [];
}
