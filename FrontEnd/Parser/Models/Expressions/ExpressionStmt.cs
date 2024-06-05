using FrontEnd.Parser.Services;

namespace FrontEnd.Parser.Models.Expressions
{
    public abstract class ExpressionStmt(string type)
    {
        protected string Type = type;

        public virtual string ToString(int indentLevel = 0)
        {
            return IndentHelper.Indent($"{Type}", indentLevel);
        }
    }
}