namespace KukuLang.Parser.Models.Expressions.Literals
{
    internal class TextLiteral(string text) : LiteralExp("Text Literal")
    {
        public string Val = text;
    }
}
