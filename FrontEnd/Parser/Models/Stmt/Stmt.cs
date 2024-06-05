using FrontEnd.Parser.Services;

namespace FrontEnd.Parser.Models.Stmt
{
    public abstract class Stmt(string type)
    {
        protected string Type = type;

        public virtual string ToString(int indentLevel = 0)
        {
            return IndentHelper.Indent($"Type: {Type}", indentLevel);
        }
    }
}