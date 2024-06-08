using FrontEnd.Parser.Models.Expressions;

namespace KukuLang.Parser.Models.Expressions.Literals
{
    public abstract class LiteralExp(string type) : ExpressionStmt(type)
    {
        public override string ToString()
        {
            return $"{GetType().Name}: {type}";
        }
    }
}
