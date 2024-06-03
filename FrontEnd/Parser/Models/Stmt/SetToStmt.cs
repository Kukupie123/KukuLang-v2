namespace FrontEnd
{
    public class SetToStmt : Stmt
    {
        public string VariableName { get; }
        public ExpressionStmt VariableValue { get; }

        public SetToStmt(string variableName, ExpressionStmt valueExp)
            : base("Set To")
        {
            VariableName = variableName;
            VariableValue = valueExp;
        }

        public override string ToString()
        {
            return $"Set {VariableName} to {VariableValue}";
        }
    }
}
