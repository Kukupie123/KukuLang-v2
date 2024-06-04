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
            var varString = string.Join(", ", VarNameVarTypePair.Select(v => $"{IndentHelper.Indent($"{v.Key}: {v.Value}", indentLevel)}"));
            return $@"
        Type Name: {TypeName},
        Variables: {varString}
    ";
        }
    }
}
