using System.Diagnostics;
using FrontEnd.Lexer;

namespace FrontEnd;

class Program
{
    static void Main(string[] args)
    {
        string sourcePath1 = "C:\\Project\\KukuLang\\KukuLang\\FrontEnd\\Snippet\\Demo.kukulang";
        string source = File.ReadAllText(sourcePath1);
        Console.WriteLine(source);
        KukuLexer lexer = new(source);
        var tokens = lexer.Tokenize();
        var parser = new RecursiveDescentParser(tokens);
        var ast = parser.Parse();
        Console.WriteLine(ast.ToString(0));
    }
}
