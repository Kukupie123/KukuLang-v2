using KukuLang.Parser.Models.Expressions.Literals;

public class BoolLiteral(bool val) : LiteralExp("Bool Literal")
{
    public bool Val = val;
}

