namespace FrontEnd
{
    /*
    Set a to 12.
    a is VariableExp
    CurrentVariable = a
    Nextnode = null
    Set Kuku's name to "Kuku".
    Kuku is VariableExp as
    CurrentVariable = Kuku
    Nextnode = name
    NextNode is used to enter deeper in variables
    */
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

        public override string ToString()
        {
            if (NextNode != null)
            {
                return $"{CurrentVariable}.{NextNode}";
            }
            else
            {
                return $"{CurrentVariable}";
            }
        }
    }
}
