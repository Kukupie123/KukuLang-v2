using FrontEnd.Parser.Services;

namespace FrontEnd.Parser.Models.Expressions
{
    /**
     * <summary>
     * Represents A property or multi level property variable such as
     * kuku or kuku.name
     * </summary>
     */
    public class NestedPropertyExp(string currentVariable, NestedPropertyExp? nextNode) : ExpressionStmt("Nested Variable Exp")
    {
        public string CurrentVariable { get; } = currentVariable;
        public NestedPropertyExp? NextNode { get; } = nextNode;

        public override string ToString(int indentLevel = 0)
        {
            if (NextNode != null)
                return IndentHelper.Indent($"{CurrentVariable}.{NextNode.ToString()}", indentLevel);
            return IndentHelper.Indent($"{CurrentVariable}", indentLevel);
        }
    }
}
