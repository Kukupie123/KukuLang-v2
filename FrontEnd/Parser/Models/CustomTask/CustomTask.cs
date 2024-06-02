namespace FrontEnd;

public class CustomTask(string taskName, string taskReturnType, Dictionary<string, string> paramNameParamTypePair)
{
    string TaskName = taskName;
    string TaskReturnType = taskReturnType;

    Dictionary<string, string> ParamNameParamTypePair = paramNameParamTypePair;
}
