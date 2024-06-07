namespace KukuLang.Interpreter.Model.RuntimeObj
{
    public class RuntimeObj
    {
        public dynamic Val; //For primitive types such as int and text this will be the value. For custom types this will be Dictionary
        public RuntimeObjType RuntimeObjType;
        public RuntimeObj(RuntimeObjType runtimeObjTypeint, int val)
        {
            RuntimeObjType = runtimeObjTypeint;
            Val = val;
        }
        public RuntimeObj(RuntimeObjType runtimeObjTypeint, float val)
        {
            RuntimeObjType = runtimeObjTypeint;

            Val = val;
        }
        public RuntimeObj(RuntimeObjType runtimeObjTypeint, string val)
        {
            RuntimeObjType = runtimeObjTypeint;

            Val = val;
        }
        public RuntimeObj(RuntimeObjType runtimeObjTypeint, Dictionary<string, RuntimeObj> val)
        {
            RuntimeObjType = runtimeObjTypeint;

            Val = val;
        }
    }
}
