using FrontEnd.Parser.Models.CustomType;
using KukuLang.Interpreter.Model.RuntimeObj;
using KukuLang.Interpreter.Model.Scope;

namespace KukuLang.Interpreter.Service
{
    public static class CustomTypeHelper
    {
        public static RuntimeObj CreateObjectFromCustomType(CustomTypeBase customType, RuntimeScope scope)
        {
            var variables = new Dictionary<string, RuntimeObj>();

            foreach (var (varName, varType) in customType.VarNameVarTypePair)
            {
                variables[varName] = varType switch
                {
                    "int" => new RuntimeObj(RuntimeObjType.Integer, 0),
                    "text" => new RuntimeObj(RuntimeObjType.Text, string.Empty),
                    _ => CreateObjectFromCustomType(scope.GetCustomType(varType), scope)
                };
            }

            return new RuntimeObj(RuntimeObjType.CustomType, variables);
        }
    }
}