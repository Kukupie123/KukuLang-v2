using FrontEnd.Parser.Services;

namespace FrontEnd.Parser.Models.Expressions
{
    public class VariableExp(string currentVariable, VariableExp? nextNode) : ExpressionStmt("Nested Variable Exp")
    {
        public string CurrentVariable { get; } = currentVariable;
        public VariableExp? NextNode { get; } = nextNode;

        public override string ToString(int indentLevel = 0)
        {
            if (NextNode != null)
                return IndentHelper.Indent($"{CurrentVariable}.{NextNode.ToString()}", indentLevel);
            return IndentHelper.Indent($"{CurrentVariable}", indentLevel);
        }
    }
}
