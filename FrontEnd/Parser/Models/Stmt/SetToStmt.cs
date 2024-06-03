namespace FrontEnd
{
    public class SetToStmt : Stmt
    {
        public VariableExp VariableName { get; }
        public ExpressionStmt VariableValue { get; }

        public SetToStmt(VariableExp variableName, ExpressionStmt valueExp)
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
