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
            // Extract the variable name from the VariableName property

            // Adjust the main part to only include the variable name and value
            var mainPart = IndentHelper.Indent($"Set {VariableName.ToString(indentLevel)} to {VariableValue.ToString(indentLevel)}", indentLevel);

            // Recursively call ToString on VariableValue to handle its indentation
            var valuePart = VariableValue.ToString(indentLevel + 2); // Increase indentation level for nested content

            // Combine the main part and the value part with a newline in between
            return $"{mainPart}\n{valuePart}";
        }
    }



}