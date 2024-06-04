namespace FrontEnd
{
    public class CustomTask : Stmt
    {
        public string TaskName { get; }
        public string TaskReturnType { get; }
        public Dictionary<string, string> ParamNameParamTypePair { get; }

        public ASTScope TaskScope;

        public CustomTask(string taskName, string taskReturnType, Dictionary<string, string> paramNameParamTypePair, ASTScope taskScope)
            : base(taskName)
        {
            TaskName = taskName;
            TaskReturnType = taskReturnType;
            ParamNameParamTypePair = paramNameParamTypePair;
            TaskScope = taskScope;
        }

        public override string ToString(int indentLevel = 0)
        {
            var paramString = string.Join(", ", ParamNameParamTypePair.Select(p => $"{IndentHelper.Indent($"{p.Key}: {p.Value}", indentLevel + 2)}"));
            var scopeString = TaskScope != null ? TaskScope.ToString(indentLevel + 2) : IndentHelper.Indent("No Scope", indentLevel + 2);

            return IndentHelper.Indent($@"Return Type: {TaskReturnType},
                                  Parameters: {paramString},
                                  Scope: {scopeString}", indentLevel);
        }
    }
}