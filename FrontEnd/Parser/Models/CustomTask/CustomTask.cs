namespace FrontEnd
{
    public class CustomTask : Stmt
    {
        public string TaskName { get; }
        public string TaskReturnType { get; }
        public Dictionary<string, string> ParamNameParamTypePair { get; }

        public CustomTask(string taskName, string taskReturnType, Dictionary<string, string> paramNameParamTypePair)
            : base("Custom Task")
        {
            TaskName = taskName;
            TaskReturnType = taskReturnType;
            ParamNameParamTypePair = paramNameParamTypePair;
        }

        public override string ToString()
        {
            var paramString = string.Join(", ", ParamNameParamTypePair.Select(p => $"{p.Key}: {p.Value}"));
            return $"Task Name: {TaskName}, Return Type: {TaskReturnType}, Parameters: {paramString}";
        }
    }
}
