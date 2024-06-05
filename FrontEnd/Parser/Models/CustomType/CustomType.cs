using FrontEnd.Parser.Services;
using Microsoft.VisualBasic;

namespace FrontEnd.Parser.Models.CustomType
{
    public class CustomType(string typeName, Dictionary<string, string> varNameVarTypePair) : Stmt.Stmt("Custom Type")
    {
        public string TypeName { get; } = typeName;
        public Dictionary<string, string> VarNameVarTypePair { get; } = varNameVarTypePair;

        public override string ToString(int indentLevel = 0)
        {
            string typeNamstr = $"{IndentHelper.Indent(TypeName, indentLevel)}\n";
            string paramsStr = $"{IndentHelper.Indent("Params: ", indentLevel + 2)}\n";
            foreach (var kvp in VarNameVarTypePair)
            {
                paramsStr += $"{IndentHelper.Indent($"{kvp.Key} : {kvp.Value}", indentLevel + 4)}\n";
            }
            var finalString = typeNamstr + paramsStr;
            return finalString + "\n";
        }
    }
}
