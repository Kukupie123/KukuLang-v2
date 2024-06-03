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

        public override string ToString()
        {
            var varString = string.Join(", ", VarNameVarTypePair.Select(v => $"{v.Key}: {v.Value}"));
            return $"Type Name: {TypeName}, Variables: {varString}";
        }
    }
}
