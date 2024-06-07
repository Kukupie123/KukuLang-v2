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
            ASTRootScope.CustomTypes.ForEach(type => { scopeTypes.Add(type.TypeName, type); });
            Dictionary<string, CustomTaskBase> scopeTasks = [];
            ASTRootScope.CustomTasks.ForEach(task => { scopeTasks.Add(task.TaskName, task); });
            var runtimeRootScope = new RuntimeScope(scopeTypes, scopeTasks, null);
            ASTRootScope.Statements.ForEach(statement => { Handler.Handler.StatementHandler(statement, runtimeRootScope); });
        }
    }
}
