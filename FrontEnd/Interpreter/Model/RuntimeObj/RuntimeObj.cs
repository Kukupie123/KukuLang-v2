namespace KukuLang.Interpreter.Model.RuntimeObj
{
    public class RuntimeObj
    {
        public dynamic Val; // For primitive types such as int and text this will be the value. For custom types this will be Dictionary
        public string RuntimeObjType;

        public RuntimeObj(string runtimeObjType, int val)
        {
            RuntimeObjType = runtimeObjType;
            Val = val;
        }

        public RuntimeObj(string runtimeObjType, float val)
        {
            RuntimeObjType = runtimeObjType;
            Val = val;
        }

        public RuntimeObj(string runtimeObjType, string val)
        {
            RuntimeObjType = runtimeObjType;
            Val = val;
        }

        public RuntimeObj(string runtimeObjType, List<dynamic> val)
        {
            RuntimeObjType = runtimeObjType;
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
