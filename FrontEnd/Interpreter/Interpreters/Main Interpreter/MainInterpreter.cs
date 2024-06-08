using FrontEnd.Parser.Models.CustomTask;
using FrontEnd.Parser.Models.CustomType;
using FrontEnd.Parser.Models.Scope;
using KukuLang.Interpreter.Model.Scope;

namespace KukuLang.Interpreter.Interpreters.Main_Interpreter
{
    public class MainInterpreter(ASTScope astRootScope)
    {
        public ASTScope ASTRootScope = astRootScope;

        public void Interpret()
        {
            Dictionary<string, CustomTypeBase> scopeTypes = [];
            //Add other root defined types
            ASTRootScope.CustomTypes.ForEach(type =>
            {
                Console.WriteLine($"Defined Type {type.ToString(0)}");

                scopeTypes.Add(type.TypeName, type);
            });

            //Create predefined tasks.
            Dictionary<string, CustomTaskBase> scopeTasks = [];
            //Add defined tasks
            ASTRootScope.CustomTasks.ForEach(task =>
            {
                Console.WriteLine($"Defined Task {task.ToString(0)}");
                scopeTasks.Add(task.TaskName, task);
            });

            var runtimeRootScope = new RuntimeScope(scopeTypes, scopeTasks, null);
            ASTRootScope.Statements.ForEach(statement => { Handler.StatementHandler.HandleStatement(statement, runtimeRootScope); });
        }
    }
}
