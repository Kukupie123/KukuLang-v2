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
                RuntimeObj runtimeObj = varType switch
                {
                    "int" => new RuntimeObj("int", 0),
                    "text" => new RuntimeObj("text", string.Empty),
                    "list" => new RuntimeObj("list", new List<dynamic>()),
                    _ => CreateObjectFromCustomType(scope.GetCustomType(varType), scope)
                };

                variables[varName] = runtimeObj;
                Console.WriteLine($"Creating variable object '{varName}' of type '{varType}' in custom type '{customType.TypeName}'");
            }

            Console.WriteLine($"Created custom object of type '{customType.TypeName}' with variables: {string.Join(", ", variables.Keys)}");

            return new RuntimeObj(customType.TypeName, variables);
        }
    }
}
