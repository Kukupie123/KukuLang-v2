namespace FrontEnd
{
    public abstract class Stmt
    {
        protected string Type;

        public Stmt(string type)
        {
            Type = type;
        }

        public virtual string ToString(int indentLevel = 0)
        {
            return IndentHelper.Indent($"Type: {Type}", indentLevel);
        }
    }
}