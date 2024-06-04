using Microsoft.VisualBasic;

namespace FrontEnd
{
    public class CustomType : Stmt
    {
        public string TypeName { get; }
        public Dictionary<string, string> VarNameVarTypePair { get; }

        public CustomType(string typeName, Dictionary<string, string> varNameVarTypePair) : base("Custom Type")
        {
            TypeName = typeName;
            VarNameVarTypePair = varNameVarTypePair;
        }

        public override string ToString(int indentLevel = 0)
        {
            string typeNamstr = $"{IndentHelper.Indent(TypeName, indentLevel)}\n";
            string paramsStr = $"{IndentHelper.Indent("Params: ", indentLevel + 2)}\n";
            foreach (var kvp in VarNameVarTypePair)
            {
                paramsStr = paramsStr + $"{IndentHelper.Indent($"{kvp.Key} : {kvp.Value}", indentLevel + 4)}\n";
            }
            var finalString = typeNamstr + paramsStr;
            return finalString + "\n";
        }
    }
}
