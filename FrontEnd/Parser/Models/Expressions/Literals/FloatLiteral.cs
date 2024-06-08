using KukuLang.Parser.Models.Expressions.Literals;

public class FloatLiteral(float val) : LiteralExp("Float Literal")
{
    public float Val = val;
}

