namespace KukuLang.Interpreter.Model.RuntimeObj
{
    public class RuntimeObj
    {
        public dynamic Val; // For primitive types such as int and text this will be the value. For custom types this will be Dictionary
        public string RuntimeObjType;

        public RuntimeObj(int val)
        {
            RuntimeObjType = "int";
            Val = val;
        }

        public RuntimeObj(float val)
        {
            RuntimeObjType = "float";
            Val = val;
        }

        public RuntimeObj(string val)
        {
            RuntimeObjType = "text";
            Val = val;
        }

        public RuntimeObj(bool val)
        {
            RuntimeObjType = "bool";
            Val = val;
        }

        public RuntimeObj(List<dynamic> val)
        {
            RuntimeObjType = "list";
            Val = val;
        }

        public RuntimeObj(string runtimeObjType, Dictionary<string, RuntimeObj> val)
        {
            RuntimeObjType = runtimeObjType;
            Val = val;
        }

        public override string ToString()
        {
            return Val switch
            {
                Dictionary<string, RuntimeObj> dict => $"{{ {string.Join(", ", dict.Select(kvp => $"{kvp.Key}: {kvp.Value}"))} }}",
                _ => Val.ToString()
            };
        }
    }
}
