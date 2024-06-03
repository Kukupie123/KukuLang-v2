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
        KukuLexer lexer = new KukuLexer(source);
        var tokens = lexer.Tokenize();
        //tokens.ForEach(t => Console.WriteLine(t));
        var prattParser = new PrattParser(tokens);
        Console.WriteLine(prattParser.Parse().ToString());
    }
}
