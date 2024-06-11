using FrontEnd.Lexer;
using KukuLang.Interpreter.Interpreters.Main_Interpreter;

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
        tokens.ForEach(token => Console.WriteLine(token));
        var parser = new RecursiveDescentParser(tokens);
        var ast = parser.Parse();
        Console.WriteLine(ast.ToString(0));
        MainInterpreter interpreter = new(ast);
        interpreter.Interpret();
        //TODO: list
        //TODO: execute else block
        //TODO: import statements.
    }
}
