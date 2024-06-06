using FrontEnd.Parser.Models.Expressions;

namespace FrontEnd.Parser.Models.Stmt
{
    public class SetToStmt(NestedPropertyExp variableName, ExpressionStmt valueExp) : Stmt("Set To")
    {
        public NestedPropertyExp VariableName { get; } = variableName;
        public ExpressionStmt VariableValue { get; } = valueExp;

        public override string ToString(int indentLevel = 0)
        {
            string variableNameStr = VariableName.ToString(indentLevel);
            string variableValStr = VariableValue.ToString(indentLevel + 2);

            return $"{variableNameStr} :\n{variableValStr}";
        }
    }



}