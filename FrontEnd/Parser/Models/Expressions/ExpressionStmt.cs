namespace FrontEnd
{
    public abstract class ExpressionStmt
    {
        protected string Type;

        public ExpressionStmt(string type)
        {
            Type = type;
        }

        public virtual string ToString(int indentLevel = 0)
        {
            return IndentHelper.Indent($"Type : {Type}", indentLevel);
        }
    }
}