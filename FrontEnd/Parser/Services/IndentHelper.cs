namespace FrontEnd;

public static class IndentHelper
{
    public static string Indent(string str, int level)
    {
        return new string(' ', level * 1) + str;
    }
}