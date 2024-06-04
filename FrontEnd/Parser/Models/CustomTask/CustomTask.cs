namespace FrontEnd
{
    public class CustomTask : Stmt
    {
        public string TaskName { get; }
        public string TaskReturnType { get; }
        public Dictionary<string, string> ParamNameParamTypePair { get; }

        public ASTScope TaskScope;

        public CustomTask(string taskName, string taskReturnType, Dictionary<string, string> paramNameParamTypePair, ASTScope taskScope)
            : base("Custom Task")
        {
            TaskName = taskName;
            TaskReturnType = taskReturnType;
            ParamNameParamTypePair = paramNameParamTypePair;
            TaskScope = taskScope;
        }

        public override string ToString()
        {
            var paramString = string.Join(", ", ParamNameParamTypePair.Select(p => $"{p.Key}: {p.Value}"));
            return $@"
        Task Name: {TaskName},
        Return Type: {TaskReturnType},
        Parameters: {paramString},
        Scope: {TaskScope}
    ";
        }
    }
}
