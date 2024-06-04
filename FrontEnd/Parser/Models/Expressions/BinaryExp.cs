namespace FrontEnd
{
    public class BinaryExp : ExpressionStmt
    {
        public ExpressionStmt Left { get; }
        public string Op { get; }
        public ExpressionStmt Right { get; }

        public BinaryExp(ExpressionStmt left, string op, ExpressionStmt right)
            : base("BinaryExp")
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public override string ToString(int indentLevel = 0)
        {
            return $"{base.ToString(indentLevel)}\n" +
                   $"{IndentHelper.Indent($"Operator: {Op}", indentLevel + 2)}\n" +
                   $"{IndentHelper.Indent($"Left:", indentLevel + 2)}\n{Left.ToString(indentLevel + 4)}\n" +
                   $"{IndentHelper.Indent($"Right:", indentLevel + 2)}\n{Right.ToString(indentLevel + 4)}";
        }
    }
}