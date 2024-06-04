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

        public override string ToString(int indentLevel = 0)
        {
            string variableNameStr = VariableName.ToString(indentLevel);
            string variableValStr = VariableValue.ToString(indentLevel + 2);

            return $"{variableNameStr} :\n{variableValStr}";
        }
    }



}