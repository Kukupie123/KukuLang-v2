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
            string taskNamestr = $"{IndentHelper.Indent(TaskName, indentLevel)}\n";

            string parameters = IndentHelper.Indent("Params:", indentLevel + 2) + "\n";
            foreach (var kv in ParamNameParamTypePair)
            {
                parameters += IndentHelper.Indent($"{kv.Key} : {kv.Value}", indentLevel + 4) + "\n";
            }
            parameters += "\n";
            string returnTypeStr = $"{IndentHelper.Indent("Returns : " + TaskReturnType, indentLevel)}\n";
            string body = IndentHelper.Indent("Statements:", indentLevel + 2) + "\n";
            body += TaskScope.ToString(indentLevel + 4);
            string finalString = taskNamestr + returnTypeStr + parameters + body;
            return finalString + "\n";
        }
    }
}