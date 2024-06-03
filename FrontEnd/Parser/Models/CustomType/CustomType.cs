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
            // Create a string that represents the variable-type pairs in a readable format.
            string varTypePairs = string.Join(", ", VarNameVarTypePair.Select(kv => $"{kv.Key}({kv.Value})"));

            // Construct and return the string representation.
            return $"{TypeName} with {varTypePairs}.";
        }
    }
}
