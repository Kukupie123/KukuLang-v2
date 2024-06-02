﻿namespace FrontEnd;

///Represents an object such as integer, float, string, custom type
public class ObjectExp(string type, Dictionary<string, ExpressionStmt> varValPair) : ExpressionStmt(type)
{
    string Type = type;
    Dictionary<string, ExpressionStmt> VarValPair = varValPair;
}
