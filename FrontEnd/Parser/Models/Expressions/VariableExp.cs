namespace FrontEnd
{
    public class VariableExp : ExpressionStmt
    {
        public string CurrentVariable { get; }
        public VariableExp? NextNode { get; }

        public VariableExp(string currentVariable, VariableExp? nextNode)
            : base("Nested Variable Exp")
        {
            CurrentVariable = currentVariable;
            NextNode = nextNode;
        }

        public override string ToString(int indentLevel = 0)
        {
            // Directly return the CurrentVariable without considering NextNode
            return IndentHelper.Indent(CurrentVariable, indentLevel);
        }
    }
}
