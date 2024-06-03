namespace FrontEnd;

public class CustomTask(string taskName, string taskReturnType, Dictionary<string, string> paramNameParamTypePair) : Stmt("Custom Task")
{
    string TaskName = taskName;
    string TaskReturnType = taskReturnType;

    Dictionary<string, string> ParamNameParamTypePair = paramNameParamTypePair;
}
