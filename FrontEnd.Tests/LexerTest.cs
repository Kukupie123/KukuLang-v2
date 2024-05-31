using System;
using FrontEnd;
using FrontEnd.Lexer;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontEnd.Tests;

[TestClass]
[TestSubject(typeof(Program))]
public class LexerTest
{

    [TestMethod]
    public void Tokenize1()
    {
        string source = "set a to 1;";
        var lexer = new KukuLexer(source);
        lexer.Tokenize().ForEach(token => Console.WriteLine(token));
        
    }    
    [TestMethod] 
    public void Tokenize2()
    {
        string source = "set a to 1;\nset b to a;";
        var lexer = new KukuLexer(source);
        lexer.Tokenize().ForEach(token => Console.WriteLine(token));
        
    }
    
    [TestMethod] 
    public void Tokenize3()
    {
        string source = "set a to 1;\nset b to a;\nif a is b then {\n    set a to b;\n}";
        var lexer = new KukuLexer(source);
        lexer.Tokenize().ForEach(token => Console.WriteLine(token));
        
    }
    
    [TestMethod] 
    public void Tokenize4()
    {
        string source = "set a to 1;\nset b to a;\nif a is b then {\n    set a to b;\n}\nuntil a is_not b repeat {\n    set a to b;\n}";
        var lexer = new KukuLexer(source);
        lexer.Tokenize().ForEach(token => Console.WriteLine(token));
        
    }
    
    [TestMethod] 
    public void Tokenize5()
    {
        string source = "set a to 1;\nset b to a;\nif a is b then {\n    set a to b;\n}\nuntil a is_not b repeat {\n    set a to b;\n}\nas_long_as a is_not b repeat {\n    set a to b;\n}";
        var lexer = new KukuLexer(source);
        lexer.Tokenize().ForEach(token => Console.WriteLine(token));
        
    }
}