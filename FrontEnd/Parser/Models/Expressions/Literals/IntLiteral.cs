﻿namespace KukuLang.Parser.Models.Expressions.Literals
{
    public class IntLiteral(int val) : LiteralExp("Int Literal")
    {
        public int Val = val;
    }
}
